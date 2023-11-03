using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitObject : MonoBehaviour
{
    public UnitBehaviour behaviour;
    public SkeletonAnimation model;

    public void InjectBehaviour(UnitBehaviour _behaviour)
    {
        behaviour = _behaviour;
    }

    private void Update()
    {
        behaviour?.Update();
    }
}
