using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempData
{
    public static TempData Instance = null;

    public TempData()
    {
        Instance = this;
    }

    public int[] curDeckUnits;
    public static void SetDeckUnit(int[] units)
    {
        Instance.curDeckUnits = units;
    }
}
