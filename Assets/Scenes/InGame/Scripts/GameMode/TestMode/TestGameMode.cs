using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestGameMode : IGameModeBehaviour
{
    InGameManager manager;
    Camera mainCam;

    GamePlayer player;
    GamePlayer enemyPlayer;

    bool isInit;

    public TestGameMode(InGameManager _manager)
    {
        manager = _manager;
    }

    public void Start()
    {
        IndexAndUUID postData = new IndexAndUUID();
        postData.uuid = UserData.Instance.uuid;
        postData.index = TempData.Instance.selectedDeck;

        mainCam = Camera.main;

        WebRequest.Post("test-ingame/game-enter", JsonUtility.ToJson(postData), (data) =>
        {
            var deck = JsonUtility.FromJson<DeckData>(data);
            List<UnitData> unitDatas = new List<UnitData>();
            foreach (var id in deck.unit_indexes)
            {
                if (id < 0) continue;
                unitDatas.Add(UserData.Instance.units.Find(unit => unit.id == id));
            }

            int stageLevel = 0;
            int waveCount = 4;
            int unitsCount = 4;
            UnitData[][] enemyDatas = new UnitData[waveCount][];
            for (int i = 0; i < waveCount; i++)
            {
                enemyDatas[i] = new UnitData[unitsCount];
                for (int j = 0; j < 4; j++)
                {
                    var unitData = new UnitData();
                    unitData.index = 0;
                    unitData.level = stageLevel;
                    enemyDatas[i][j] = unitData;
                }
            }

            player = new PlayableGamePlayer(unitDatas.ToArray(), UnitGroupType.ALLY, manager.skillCanvas);
            enemyPlayer = new TestEnemyGamePlayer(enemyDatas, UnitGroupType.HOSTILE, stageLevel);

            InGameManager.Instance.StartCoroutine(GameLogic());
            isInit = true;
        });
    }

    public void Update()
    {
        if (!isInit) return;

        player.Update();
        enemyPlayer.Update();

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
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        int waveCount = 0;
        (player as PlayableGamePlayer).ActiveAllUnits();
        player.StartGame();
        enemyPlayer.StartGame();

        while (waveCount < 4)
        {
            (enemyPlayer as TestEnemyGamePlayer).ActiveUnitAt(waveCount);
            while (enemyPlayer.GetCountOfUnits() > 0)
            {
                if (player.GetCountOfUnits() <= 0)
                {
                    player.isGameActive = false;
                    // post lose event
                }
                yield return null;
            }

            yield return new WaitUntil((player as PlayableGamePlayer).EveryUnitActionEnds);
            waveCount++;
        }

        // post win event
        yield break;
    }

    class IndexAndUUID
    {
        public string uuid;
        public int index;
    }
}
