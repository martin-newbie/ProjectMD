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
        characters = Resources.LoadAll<SkeletonDataAsset>("SkeletonDatas/Characters");

        var profiles = Resources.LoadAll<Sprite>("Sprites/Profiles");
        characterProfiles = profiles.OrderBy((item) => int.Parse(item.name.Split('-')[0])).ToArray();
    }

    SkeletonDataAsset[] characters;
    Sprite[] characterProfiles;

    public static SkeletonDataAsset GetSkeleton(int idx)
    {
        if (idx >= Instance.characters.Length - 1) idx = 0;
        return Instance.characters[idx];
    }

    public static Sprite GetProfile(int idx)
    {
        if (idx >= Instance.characterProfiles.Length - 1) idx = 0;
        return Instance.characterProfiles[idx];
    }
}
