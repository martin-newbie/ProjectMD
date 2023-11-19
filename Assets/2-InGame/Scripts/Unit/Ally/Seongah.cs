using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seongah : ActiveSkillBehaviour
{
    public Seongah(UnitObject _subject) : base(_subject)
    {
        maxAmmo = 5;
        curAmmo = maxAmmo;
    }

    protected override IEnumerator AttackLogic()
    {
        var target = GetNearestOpponent();
        ShootBullet(target);
        
        yield return PlayAnimAndWait("battle_attack");
        yield return PlayAnimAndWait("battle_bolt");
        yield break;
    }

    public override IEnumerator ActiveSkill(ActiveSkillValue skillData)
    {
        yield break;
    }

    public override void CollabseSkill(ActiveSkillValue skillData, UnitBehaviour subjectUnit)
    {
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
