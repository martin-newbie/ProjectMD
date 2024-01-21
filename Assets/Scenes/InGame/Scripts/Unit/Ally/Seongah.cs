using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seongah : ActiveSkillBehaviour
{
    public Seongah(UnitObject _subject) : base(_subject)
    {
    }

    protected override IEnumerator AttackLogic()
    {
        var target = GetNearestOpponent();
        ShootBullet(target);
        
        yield return PlayAnimAndWait("battle_attack");
        yield return PlayAnimAndWait("battle_bolt");
        yield break;
    }

    public override IEnumerator ActiveSkill(DamageStruct skillData)
    {
        yield break;
    }

    public override void CollabseBuff(DamageStruct skillData, UnitBehaviour subjectUnit)
    {
    }

    public override bool ActiveSkillCondition()
    {
        return false;
    }
}
