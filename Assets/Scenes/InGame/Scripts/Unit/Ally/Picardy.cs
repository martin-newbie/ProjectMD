using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picardy : ActiveSkillBehaviour
{
    public Picardy(UnitObject _subject) : base(_subject)
    {
    }

    public override IEnumerator ActiveSkill(DamageStruct skillData)
    {
        throw new System.NotImplementedException();
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
        var target = GetNearestOpponent();
        ShootBullet(target);

        yield return PlayAnimAndWait("battle_attack");
    }
}
