using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enforcer_Tanker : UnitBehaviour
{
    public Enforcer_Tanker(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
    {
    }

    protected override IEnumerator AttackLogic()
    {
        yield return CommonBurstFire(3);
    }
}
