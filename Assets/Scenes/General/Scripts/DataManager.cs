using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance = null;

    public static string chapterKey = "chapter";

    public List<ChapterData> chapters = new List<ChapterData>();

    private void Awake()
    {
        Instance = this;
        WebRequest.Post("data/game-data", ""/*version require*/, (data) =>
        {
            var recieveData = JsonUtility.FromJson<GameData>(data);
        });
        Load();
    }

    void Load()
    {
        chapters = new List<ChapterData>();
        int chapterCount = 1;
        for (int i = 0; i < chapterCount; i++)
        {
            var chapterTextAsset = Resources.Load<TextAsset>($"Stages/{i}");
            var chapter = JsonUtility.FromJson<ChapterData>(chapterTextAsset.text);
            chapters.Add(chapter);
        }
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