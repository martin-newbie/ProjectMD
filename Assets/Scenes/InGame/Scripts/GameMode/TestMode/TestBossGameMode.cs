using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBossGameMode : IGameModeBehaviour
{
    int[] spawnIdx = { 33, 30, 27, 24, 21 };
    InGameManager manager;

    GamePlayer player;
    GamePlayer enemyPlayer;

    bool isInit = false;

    public TestBossGameMode(InGameManager _manager)
    {
        manager = _manager;
    }

    public void Start()
    {

        IndexAndUUID postData = new IndexAndUUID();
        postData.uuid = UserData.Instance.uuid;
        postData.index = TempData.Instance.selectedDeck;

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
            var bossData = new UnitData();
            bossData.skill_level = new int[4];

            player = new PlayableGamePlayer(unitDatas.ToArray(), UnitGroupType.ALLY, manager.skillCanvas);
            enemyPlayer = new TestBossGamePlayer(bossData, UnitGroupType.HOSTILE, stageLevel);

            InGameManager.Instance.StartCoroutine(GameLogic());
            isInit = true;
        });
    }

    IEnumerator GameLogic()
    {
        (player as PlayableGamePlayer).ActiveAllUnits();
        player.StartGame();
        enemyPlayer.StartGame();

        yield return new WaitUntil(() => enemyPlayer.curUnits.Count <= 0);

        player.isGameActive = false;
        enemyPlayer.isGameActive = false;
        yield break;
    }


    public void Update()
    {
        if (!isInit) return;

        player.Update();
        enemyPlayer.Update();
    }
}
