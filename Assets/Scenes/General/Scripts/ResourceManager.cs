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
        
    }

    public static SkeletonDataAsset GetSkeleton(int idx)
    {
        return Instance.characters[idx];
    }

    public static Sprite GetProfile(int idx)
    {
        return Instance.characterProfiles[idx];
    }
}
