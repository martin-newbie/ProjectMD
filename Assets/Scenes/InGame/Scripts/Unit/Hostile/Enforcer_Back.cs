using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enforcer_Back : UnitBehaviour
{
    public Enforcer_Back(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
    {
    }

    protected override IEnumerator AttackLogic()
    {
        yield return CommonBurstFire(1);
    }
}
