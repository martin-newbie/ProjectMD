using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance = null;
    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < 100; i++)
        {
            var spine = Resources.Load<SkeletonDataAsset>("SkeletonDatas/Characters/" + i.ToString() + "/skeleton_SkeletonData");
            if (spine != null)
            {
                characters.Add(i, spine);
            }
        }

        var profiles = Resources.LoadAll<Sprite>("Sprites/Profiles");
        unitProfiles = profiles.OrderBy((item) => int.Parse(item.name.Split('-')[0])).ToArray();

        var unitLevelTextAsset = Resources.Load<TextAsset>("Datas/unitLevelExp.txt");
        var levelupExp = unitLevelTextAsset.text.Split('\n').Select(item => int.Parse(item));
        unitLevelupExp = new List<int>(levelupExp);
    }

    Dictionary<int, SkeletonDataAsset> characters = new Dictionary<int, SkeletonDataAsset>();
    Sprite[] unitProfiles;
    List<int> unitLevelupExp;

    public static SkeletonDataAsset GetSkeleton(int idx)
    {
        if (idx >= 0)
            return Instance.characters[idx];
        else
            return null;
    }

    public static Sprite GetUnitProfile(int idx)
    {
        if (idx > Instance.unitProfiles.Length - 1)
            idx = 0;
        return Instance.unitProfiles[idx];
    }

    public static int GetUnitLevelupExp(int level)
    {
        return Instance.unitLevelupExp[level];
    }

    public static Sprite GetItemIcon(int index)
    {
        return null;
    }
    public static Sprite GetCurrencyIcon(int index)
    {
        return null;
    }

    public Sprite dieselRobotSmg;
    public Sprite dieselRobotMelee;
}
