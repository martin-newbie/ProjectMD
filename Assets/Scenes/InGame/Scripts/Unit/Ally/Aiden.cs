using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiden : SkillBehaviour
{
    public Aiden(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
    {
    }

    public override IEnumerator ActiveSkill(SkillData skillData)
    {
        yield break;
    }

    public override bool GetActiveSkillCondition()
    {
        return false;
    }

    public override void CollabseBuff(SkillData skillData, UnitBehaviour subjectUnit)
    {
    }

    protected override IEnumerator AttackLogic()
    {
        yield return StartCoroutine(CommonBurstFire(5));
    }

}
