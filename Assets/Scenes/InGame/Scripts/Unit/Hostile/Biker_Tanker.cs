using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biker_Tanker : UnitBehaviour
{
    public Biker_Tanker(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
    {
    }

    protected override IEnumerator AttackLogic()
    {
        yield return CommonBurstFire(3);
    }
}
