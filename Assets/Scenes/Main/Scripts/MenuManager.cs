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
}
