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

    public StageGameMode(InGameManager _manager, RecieveGameEnter data)
    {
        manager = _manager;
        mainCam = Camera.main;

        stageData = StageManager.Instance.GetStageData(data.selected_chapter, data.selected_stage);
        
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

        var sendData = JsonUtility.ToJson(new SendStageResultData(timeCondition, unitCondition, winCondition));
        WebRequest.Post("ingame/game-result", sendData, (data) =>
        {
            var recieveData = JsonUtility.FromJson<RecieveStageResultData>(data);

        });
    }
}

[System.Serializable]
class SendStageResultData
{
    public bool condition1;
    public bool condition2;
    public bool condition3;

    public SendStageResultData(bool con1, bool con2, bool con3)
    {
        condition1 = con1;
        condition2 = con2;
        condition3 = con3;
    }
}

[System.Serializable]
class RecieveStageResultData
{
    public List<(int, int)> result_items;
}