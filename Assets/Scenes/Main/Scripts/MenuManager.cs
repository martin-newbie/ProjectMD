using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public MenuUnitManager menuUnit;

    private void Start()
    {
        // get furniture data from user data

        int[] testArr = new int[5] { 0, 1, 2, 3, 4 };
        menuUnit.InitUnits(testArr);
    }

    public void OnUnitListButton()
    {
        var sendData = new SendUserData();
        sendData.uuid = UserData.Instance.uuid;
        WebRequest.Post("main-menu/enter-list", JsonUtility.ToJson(sendData), (data) =>
        {
            var units = JsonUtility.FromJson<RecieveUnitList>(data);
            SceneLoadManager.Instance.LoadScene("List", () => { UnitListManager.Instance.InitList(units.units); });
        });
    }

    public void OnLoadoutButton()
    {
        var sendData = new SendUserData();
        sendData.uuid = UserData.Instance.uuid;
        WebRequest.Post("main-menu/enter-loadout", JsonUtility.ToJson(sendData), (data) =>
        {
            var decks = JsonUtility.FromJson<RecieveDeckData>(data);
            TempData.Instance.selectedGameMode = GameMode.NOTHING;
            SceneLoadManager.Instance.LoadScene("Loadout", () => { LoadoutManager.Instance.InitLoadout(decks); });
        });
    }
}

[Serializable]
public class SendUserData
{
    public string uuid;
}

[Serializable]
public class RecieveUnitList
{
    public UnitData[] units;
}