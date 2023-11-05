using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitObject : MonoBehaviour
{
    public UnitBehaviour behaviour;
    public SkeletonAnimation model;

    public void InjectBehaviour(UnitBehaviour _behaviour, SkeletonDataAsset skeleton, UnitGroupType group)
    {
        behaviour = _behaviour;
    
        model.Update(0f);
        model.skeletonDataAsset = skeleton;
        model.Initialize(true);
        
        behaviour.group = group;
    }

    private void Update()
    {
        behaviour?.Update();
    }
}
