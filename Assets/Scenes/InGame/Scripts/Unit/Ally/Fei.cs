using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fei : ActiveSkillBehaviour
{
    public Fei(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
    {
    }

    public override IEnumerator ActiveSkill(DamageStruct skillData)
    {
        yield break;
    }

    public override bool ActiveSkillCondition()
    {
        return false;
    }

    public override void CollabseBuff(DamageStruct skillData, UnitBehaviour subjectUnit)
    {
    }

    protected override IEnumerator AttackLogic()
    {
        yield return StartCoroutine(CommonBurstFire(3));
    }

}
