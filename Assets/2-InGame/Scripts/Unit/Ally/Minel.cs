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
        for (int i = 0; i < 3; i++)
        {
            var target = GetNearestOpponent();
            ShootBullet(target);
            PlayAnim("battle_attack");
            yield return new WaitForSeconds(0.15f);
        }
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
