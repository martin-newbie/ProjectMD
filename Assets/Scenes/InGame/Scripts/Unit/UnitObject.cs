using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitObject : MonoBehaviour
{
    public UnitBehaviour behaviour;
    public SkeletonAnimation model;
    public SpriteRenderer spriteModel;
    public ParticleSystem hpRecoveryEffect;

    [Header("Prob")]
    public Bullet probBullet;

    public void SetBehaviour(UnitBehaviour _behaviour, UnitGroupType group, int barType)
    {
        behaviour = _behaviour;
        behaviour.InitObject(this, barType);

        model.Update(0f);
        model.skeletonDataAsset = ResourceManager.GetSkeleton(behaviour.constData.modelIndex);
        model.Initialize(true);

        behaviour.group = group;
        behaviour.SetModelRotByDir((int)group);
    }

    private void Update()
    {
        behaviour?.Update();
    }
}
