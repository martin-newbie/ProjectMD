using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummy : UnitBehaviour
{
    public TrainingDummy(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
    {
    }

    public override void InitObject(UnitObject _subject, int barType)
    {
        base.InitObject(_subject, barType);
        hp = 100000;
    }

    protected override void InCombatFunc()
    {
        return;
    }

    protected override IEnumerator AttackLogic()
    {
        yield break;
    }
}
