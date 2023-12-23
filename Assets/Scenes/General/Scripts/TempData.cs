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
}
