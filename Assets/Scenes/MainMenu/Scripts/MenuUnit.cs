using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUnit : MonoBehaviour
{
    public SkeletonGraphic skeleton;

    public void InitUnit(int idx)
    {
        skeleton.Update(0f);
        skeleton.skeletonDataAsset = ResourceManager.GetSkeleton(idx);
        skeleton.Initialize(true);

        PlayAni("wait");
    }

    void PlayAni(string key, bool loop = false)
    {
        skeleton.AnimationState.SetAnimation(0, key, loop);
    }
}
