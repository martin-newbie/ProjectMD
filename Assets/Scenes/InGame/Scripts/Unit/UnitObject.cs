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

    public void InjectBehaviour(UnitBehaviour _behaviour, int idx, UnitGroupType group)
    {
        behaviour = _behaviour;
        behaviour.InitCommon(idx);
    
        model.Update(0f);
        model.skeletonDataAsset = InGameManager.Instance.humanoidDataAsset[idx];
        model.Initialize(true);
        
        behaviour.group = group;
        if (group == UnitGroupType.HOSTILE) behaviour.SetModelRotByDir(-1);
    }

    private void Update()
    {
        behaviour?.Update();
    }
}