using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nina : ActiveSkillBehaviour
{
    public Nina(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
    {
    }

    protected override IEnumerator MoveToTargetRange()
    {
        yield return PlayAnimAndWait("battle_move-wait");
        yield return base.MoveToTargetRange();
    }

    protected override IEnumerator AttackLogic()
    {
        yield return StartCoroutine(CommonBurstFire(3));
    }

    public override void CollabseBuff(DamageStruct skillData, UnitBehaviour subjectUnit)
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
}
