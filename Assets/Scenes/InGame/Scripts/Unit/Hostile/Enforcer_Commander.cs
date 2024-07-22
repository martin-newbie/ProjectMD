using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enforcer_Commander : UnitBehaviour
{
    public Enforcer_Commander(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
    {
    }

    protected override IEnumerator AttackLogic()
    {
        throw new System.NotImplementedException();
    }
}
