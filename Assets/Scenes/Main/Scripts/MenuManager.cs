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
        SceneLoadManager.Instance.LoadScene("List");
    }

    public void OnLoadoutButton()
    {
        TempData.Instance.selectedGameMode = GameMode.NOTHING;
        SceneLoadManager.Instance.LoadScene("Loadout");
    }
}
