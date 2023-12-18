using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitObject : MonoBehaviour
{
    public UnitBehaviour behaviour;
    public SkeletonAnimation model;

    [Header("Prob")]
    public Bullet probBullet;

    public void SetBehaviour(UnitBehaviour _behaviour, int idx, UnitGroupType group, int barType)
    {
        behaviour = _behaviour;
        behaviour.InitCommon(idx, barType);

        model.Update(0f);
        model.skeletonDataAsset = ResourceManager.GetSkeleton(idx);
        model.Initialize(true);

        behaviour.group = group;
        behaviour.SetModelRotByDir((int)group);
    }

    private void Update()
    {
        behaviour?.Update();
    }
}
