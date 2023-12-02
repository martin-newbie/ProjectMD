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
            if(spine != null)
            {
                characters.Add(i, spine);
            }
        }

        var profiles = Resources.LoadAll<Sprite>("Sprites/Profiles");
        characterProfiles = profiles.OrderBy((item) => int.Parse(item.name.Split('-')[0])).ToArray();
    }

    Dictionary<int, SkeletonDataAsset> characters = new Dictionary<int, SkeletonDataAsset>();
    Sprite[] characterProfiles;

    public static SkeletonDataAsset GetSkeleton(int idx)
    {
        return Instance.characters[idx];
    }

    public static Sprite GetProfile(int idx)
    {
        if (idx >= Instance.characterProfiles.Length - 1) idx = 0;
        return Instance.characterProfiles[idx];
    }
}
