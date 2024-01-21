using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiden : ActiveSkillBehaviour
{
    public Aiden(UnitObject _subject) : base(_subject)
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
        yield return StartCoroutine(CommonBurstFire(5));
    }

}
