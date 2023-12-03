using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lada : ActiveSkillBehaviour
{
    public Lada(UnitObject _subject) : base(_subject)
    {
    }

    public override IEnumerator ActiveSkill(ActiveSkillValue skillData)
    {
        yield break;
    }

    public override bool ActiveSkillCondition()
    {
        return false;
    }

    public override void CollabseSkill(ActiveSkillValue skillData, UnitBehaviour subjectUnit)
    {
    }

    protected override IEnumerator AttackLogic()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator PassiveSkillActive()
    {
        yield break;
    }

    protected override bool PassiveSkillCondition()
    {
        return false;
    }
}
