using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asis : ActiveSkillBehaviour
{
    public Asis(UnitObject _subject) : base(_subject)
    {
    }

    protected override IEnumerator MoveToTargetRange()
    {
        yield return PlayAnimAndWait("wp_turn");
        yield return base.MoveToTargetRange();
    }

    protected override IEnumerator AttackLogic()
    {
        // shoot grenade not bullet
        yield return PlayAnimAndWait("battle_attack");
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
}
