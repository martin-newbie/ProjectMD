using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance = null;

    public static string chapterKey = "chapter";

    public List<ChapterData> chapters = new List<ChapterData>();
    public GameData gameData;

    private void Start()
    {
        Instance = this;
        WebRequest.Post("data/game-data", "", (data) =>
        {
            var recieveData = JsonUtility.FromJson<GameData>(data);
            gameData = recieveData;

            int chapterCount = 1;
            for (int i = 0; i < chapterCount; i++)
            {
                var chapterDataText = Resources.Load<TextAsset>($"Stages/{i}");
                chapters.Add(JsonUtility.FromJson<ChapterData>(chapterDataText.text));
            }
        });
    }

    public static SkillItemRequire GetCommonSkillItem(int level)
    {
        return Instance.gameData.common_skill_data.skillItemDatas[level];
    }

    public static SkillItemRequire GetActiveSkillItem(int level)
    {
        return Instance.gameData.active_skill_data.skillItemDatas[level];
    }

    public static TierUpgrade GetTierItem(int level)
    {
        return Instance.gameData.tier_upgrade_data.upgrade_data[level];
    }

    public static int GetUserLevelExp(int level)
    {
        return Instance.gameData.user_exp_data.exp[level];
    }

    public static int GetUnitLevelExp(int level)
    {
        return Instance.gameData.unit_exp_data.exp[level];
    }

    public static int GetEquipmentLevelExp(int level)
    {
        return Instance.gameData.equipment_exp_data.exp[level];
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