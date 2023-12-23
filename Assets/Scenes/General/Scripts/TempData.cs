using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempData
{
    public static TempData Instance = null;

    public TempData()
    {
        Instance = this;
        allDeckUnits = new List<int[]>();
        for (int i = 0; i < 4; i++)
        {
            allDeckUnits.Add(new int[0]);
        }
    }

    public List<int[]> allDeckUnits;
    public static void SetDeckUnitAt(int[] units, int show)
    {
        Instance.allDeckUnits[show] = units;
        // 어차피 앞에서부터 배치다
    }

    public bool AlreadySelected(int unitId)
    {
        for (int i = 0; i < allDeckUnits.Count; i++)
        {
            var deck = allDeckUnits[i];
            for (int j = 0; j < deck.Length; j++)
            {
                if (deck[i] == unitId)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
