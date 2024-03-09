using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpineUtil
{
    public static void UpdateSkeleton(this SkeletonGraphic model, SkeletonDataAsset asset)
    {
        model.Update(0f);
        model.skeletonDataAsset = asset;
        model.Initialize(true);
    }
}
