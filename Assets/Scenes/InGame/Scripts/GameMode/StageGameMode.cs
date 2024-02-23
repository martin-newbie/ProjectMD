using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageGameMode : IGameModeBehaviour
{
    GamePlayer player;
    GamePlayer enemy;

    Camera mainCam;
    InGameManager manager;

    bool isInit = false;

    StageData stageData;


    int startingAllyUnitCount;
    float gameActivatingTime;
    float perfactTime = 120f;

    int stageIndex;
    int chapterIndex;
    int deckIndex;

    public StageGameMode(InGameManager _manager, RecieveGameEnter data)
    {
        manager = _manager;
        mainCam = Camera.main;

        stageIndex = data.selected_stage;
        chapterIndex = data.selected_chapter;
        deckIndex = TempData.Instance.selectedDeck;
        stageData = StageManager.Instance.GetStageData(chapterIndex, stageIndex);
        
        List<UnitData> unitDatas = new List<UnitData>();
        foreach (var id in data.deck.unit_indexes)
        {
            if (id < 0) continue;

            unitDatas.Add(UserData.Instance.units.Find(unit => unit.id == id));
            startingAllyUnitCount++;
        }

        player = new PlayableGamePlayer(unitDatas.ToArray(), UnitGroupType.ALLY, manager.skillCanvas);
        enemy = new StageEnemyPlayer(UnitGroupType.HOSTILE, stageData);
        isInit = true;

        InGameManager.Instance.StartCoroutine(GameLogic());
    }

    public void Start()
    {
    }

    public void Update()
    {
        if (!isInit) return;

        player.Update();
        enemy.Update();

        if (player.isGameActive && player.curUnits.Count > 0)
        {
            var frontUnit = player.curUnits.OrderByDescending(item => item.transform.position.x).ElementAt(0).transform.position;
            var frontPos = frontUnit.x;
            var camPos = new Vector3(frontPos + 3, 0f, -10f);
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, camPos, Time.deltaTime * 15f);

            gameActivatingTime += Time.deltaTime;
        }
    }

    IEnumerator GameLogic()
    {
        // game start effect
        yield return new WaitForSeconds(1f);

        int waveCount = 0;
        (player as PlayableGamePlayer).ActiveAllUnits();
        player.StartGame();
        enemy.StartGame();

        while (waveCount < stageData.waveDatas.Count)
        {
            (enemy as StageEnemyPlayer).ActiveUnitAt(waveCount);
            while (enemy.GetCountOfUnits() > 0)
            {
                if (player.GetCountOfUnits() <= 0)
                {
                    player.isGameActive = false;
                    PostResultEvent(false); // lose
                    goto StageEndPoint;
                }
                yield return null;
            }

            yield return new WaitUntil((player as PlayableGamePlayer).EveryUnitActionEnds);
            waveCount++;
        }

        PostResultEvent(true); // wind
        StageEndPoint:
        player.isGameActive = false;
        enemy.isGameActive = false;
        yield break;
    }

    void PostResultEvent(bool isWin)
    {
        bool timeCondition = isWin && gameActivatingTime < perfactTime + 1f;
        bool unitCondition = isWin && player.curUnits.Count == startingAllyUnitCount;
        bool winCondition = isWin;

        var sendResult = new SendStageResultData();
        sendResult.uuid = UserData.Instance.uuid;
        sendResult.stage_index = stageIndex;
        sendResult.chapter_index = chapterIndex;
        sendResult.deck_index = deckIndex;
        sendResult.use_energy = 10;
        sendResult.perfaction = new bool[] { timeCondition, unitCondition, winCondition };
        WebRequest.Post("ingame/game-end", JsonUtility.ToJson(sendResult), (data) =>
        {
            var recieveData = JsonUtility.FromJson<RecieveStageResultData>(data);

        });
    }
}

[System.Serializable]
class SendStageResultData
{
    public string uuid;
    public int stage_index;
    public int chapter_index;
    public int deck_index;
    public int use_energy;
    public bool[] perfaction;
}

[System.Serializable]
class RecieveStageResultData
{
    public List<(int, int)> result_items;
}