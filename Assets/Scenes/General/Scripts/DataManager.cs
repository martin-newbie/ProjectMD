using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance = null;

    public static string chapterKey = "chapter";

    public List<ChapterData> chapters = new List<ChapterData>();
    public GameData gameData;

    private void Awake()
    {
        Instance = this;
        WebRequest.Post("data/game-data", "", (data) =>
        {
            var recieveData = JsonUtility.FromJson<GameData>(data);
            gameData = recieveData;
        });
    }

    public SkillItemRequire GetCommonSkillItem(int level)
    {
        return gameData.common_skill_data.skillItemDatas[level];
    }

    public SkillItemRequire GetActiveSkillItem(int level)
    {
        return gameData.active_skill_data.skillItemDatas[level];
    }

    public TierUpgrade GetTierItem(int level)
    {
        return gameData.tier_upgrade_data.upgrade_data[level];
    }

    public int GetUserLevelExp(int level)
    {
        return gameData.user_exp_data.exp[level];
    }

    public int GetUnitLevelExp(int level)
    {
        return gameData.unit_exp_data.exp[level];
    }

    public int GetEquipmentLevelExp(int level)
    {
        return gameData.equipment_exp_data.exp[level];
    }
}

[System.Serializable]
public class GameData
{
    public CommonSkillItemRequire common_skill_data;
    public ActiveSkillItemRequire active_skill_data;
    public TierUpgradeData tier_upgrade_data;
    public UserExpData user_exp_data;
    public UnitExpData unit_exp_data;
    public EquipmentExpData equipment_exp_data;
}

[System.Serializable]
public class CommonSkillItemRequire
{
    public SkillItemRequire[] skillItemDatas;
}

[System.Serializable]
public class ActiveSkillItemRequire
{
    public SkillItemRequire[] skillItemDatas;
}

[System.Serializable]
public class SkillItemRequire
{
    public int coin;
    public ItemData[] items;
}

[System.Serializable]
public class TierUpgradeData
{
    public TierUpgrade[] upgrade_data;
}

[System.Serializable]
public class TierUpgrade
{
    public int item_require;
    public int coin_require;
}

[System.Serializable]
public class UnitExpData
{
    public int[] exp;
}

[System.Serializable]
public class UserExpData
{
    public int[] exp;
}

[System.Serializable]
public class EquipmentExpData
{
    public int[] exp;
}