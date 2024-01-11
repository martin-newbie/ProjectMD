using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minel : ActiveSkillBehaviour
{
    public Minel(UnitObject _subject) : base(_subject)
    {
    }

    protected override IEnumerator AttackLogic()
    {
        yield return StartCoroutine(CommonBurstFire(3));
    }

    public override IEnumerator ActiveSkill(ActiveSkillValue skillData)
    {
        yield break;
    }

    public override void CollabseSkill(ActiveSkillValue skillData, UnitBehaviour subjectUnit)
    {
    }

    public override bool ActiveSkillCondition()
    {
        return false;
    }
}
