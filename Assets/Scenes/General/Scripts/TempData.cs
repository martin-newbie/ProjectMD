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

    public int selectedDeck;
    public GameMode selectedGameMode;

    public int selectedUnit;
}

public static class TempDataUtil
{
    public static string ToKey(this GameMode mode, string controllerUrl = "")
    {
        if (!string.IsNullOrEmpty(controllerUrl))
        {
            controllerUrl = "-" + controllerUrl;
        }

        return mode.ToString().ToLower() + controllerUrl;
    }
}

public enum GameMode
{
    NOTHING,
    TEST,
    STAGE,
    DUNGEON,
    RAID,
    PVP
}

