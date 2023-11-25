using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance = null;
    private void Awake()
    {
        Instance = this;
    }

    SkeletonDataAsset[] characters;
    Sprite[] characterProfiles;

    private void Start()
    {
        characters = Resources.LoadAll<SkeletonDataAsset>("SkeletonDatas/Character");
        characterProfiles = Resources.LoadAll<Sprite>("Sprites/Profiles");
    }

    public static SkeletonDataAsset GetSkeleton(int idx)
    {
        if (idx >= Instance.characters.Length) idx = 0;
        return Instance.characters[idx];
    }

    public static Sprite GetProfile(int idx)
    {
        if (idx >= Instance.characterProfiles.Length) idx = 0;
        return Instance.characterProfiles[idx];
    }
}
