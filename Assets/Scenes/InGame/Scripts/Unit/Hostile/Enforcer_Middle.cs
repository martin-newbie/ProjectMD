using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enforcer_Middle : UnitBehaviour
{
    public Enforcer_Middle(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
    {
    }

    protected override IEnumerator AttackLogic()
    {
        yield return CommonBurstFire(3);
    }
}
