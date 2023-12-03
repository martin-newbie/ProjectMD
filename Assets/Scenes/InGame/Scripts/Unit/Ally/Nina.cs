using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nina : ActiveSkillBehaviour
{
    public Nina(UnitObject _subject) : base(_subject)
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

    public override void CollabseSkill(ActiveSkillValue skillData, UnitBehaviour subjectUnit)
    {
    }

    public override IEnumerator ActiveSkill(ActiveSkillValue skillData)
    {
        yield break;
    }

    protected override bool PassiveSkillCondition()
    {
        return false;
    }

    protected override IEnumerator PassiveSkillActive()
    {
        yield break;
    }

    public override bool ActiveSkillCondition()
    {
        return false;
    }
}
