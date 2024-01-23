using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestGameMode : IGameModeBehaviour
{
    InGameManager manager;

    GamePlayer player;
    GamePlayer enemyPlayer;

    public TestGameMode(InGameManager _manager)
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
            List<UnitData> unitData = new List<UnitData>();
            foreach (var id in deck.unit_indexes)
            {
                if (id < 0) continue;
                unitData.Add(UserData.Instance.units.Find(unit => unit.id == id));
            }

            player = new PlayableGamePlayer(unitData.ToArray(), UnitGroupType.ALLY, manager.skillCanvas);

        });
    }

    public void Update()
    {

    }

    class IndexAndUUID
    {
        public string uuid;
        public int index;
    }
}
