using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nina : ActiveSkillBehaviour
{
    public Nina(UnitObject _subject) : base(_subject)
    {
        maxAmmo = 30;
        curAmmo = maxAmmo;
    }

    protected override IEnumerator MoveToTargetRange()
    {
        yield return PlayAnimAndWait("battle_move-wait");
        yield return base.MoveToTargetRange();
    }

    protected override IEnumerator AttackLogic()
    {
        for (int i = 0; i < 3; i++)
        {
            var target = GetNearestOpponent();
            ShootBullet(target);
            PlayAnim("battle_attack");
            yield return new WaitForSeconds(0.15f);
        }
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