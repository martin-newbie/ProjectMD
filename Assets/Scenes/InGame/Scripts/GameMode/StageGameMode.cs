using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGameMode : IGameModeBehaviour
{
    GamePlayer player;
    GamePlayer enemy;

    Camera mainCam;
    InGameManager manager;

    bool isInit = false;

    public void Start()
    {
        var stageData = StageManager.Instance.GetStageData(TempData.Instance.selectedChapter, TempData.Instance.selectedStage);


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
            isInit = true;
        });
    }

    public void Update()
    {

    }
}
