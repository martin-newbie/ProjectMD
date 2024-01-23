using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minel : ActiveSkillBehaviour
{
    public Minel(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
    {
    }

    protected override IEnumerator AttackLogic()
    {
        yield return StartCoroutine(CommonBurstFire(3));
    }

    public override IEnumerator ActiveSkill(DamageStruct skillData)
    {
        yield break;
    }

    public override void CollabseBuff(DamageStruct skillData, UnitBehaviour subjectUnit)
    {
    }

    public override bool ActiveSkillCondition()
    {
        return false;
    }
}
