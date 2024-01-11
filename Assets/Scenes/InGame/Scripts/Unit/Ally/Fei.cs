using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fei : ActiveSkillBehaviour
{
    public Fei(UnitObject _subject) : base(_subject)
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
        yield return StartCoroutine(CommonBurstFire(3));
    }

}
