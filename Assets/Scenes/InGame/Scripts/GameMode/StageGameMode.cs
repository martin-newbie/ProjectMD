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

    public StageGameMode(InGameManager _manager)
    {
        manager = _manager;
    }

    public void Start()
    {
        stageData = StageManager.Instance.GetStageData(TempData.Instance.selectedChapter, TempData.Instance.selectedStage);

        IndexAndUUID postData = new IndexAndUUID();
        postData.uuid = UserData.Instance.uuid;
        postData.index = TempData.Instance.selectedDeck;

        mainCam = Camera.main;

        WebRequest.Post("ingame/game-enter", JsonUtility.ToJson(postData), (data) =>
        {
            var deck = JsonUtility.FromJson<DeckData>(data);
            List<UnitData> unitDatas = new List<UnitData>();
            foreach (var id in deck.unit_indexes)
            {
                if (id < 0) continue;
                unitDatas.Add(UserData.Instance.units.Find(unit => unit.id == id));
            }

            player = new PlayableGamePlayer(unitDatas.ToArray(), UnitGroupType.ALLY, manager.skillCanvas);
            enemy = new StageEnemyPlayer(UnitGroupType.HOSTILE, stageData);
            isInit = true;

            InGameManager.Instance.StartCoroutine(GameLogic());
        });
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
                    // play lose event
                }
                yield return null;
            }

            yield return new WaitUntil((player as PlayableGamePlayer).EveryUnitActionEnds);
            waveCount++;
        }
        
        // play win event
        player.isGameActive = false;
        enemy.isGameActive = false;
        yield break;
    }

}
