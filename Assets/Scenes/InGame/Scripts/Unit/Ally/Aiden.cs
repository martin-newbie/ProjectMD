using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiden : ActiveSkillBehaviour
{
    public Aiden(UnitObject _subject) : base(_subject)
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
        for (int i = 0; i < 5; i++)
        {
            var target = GetNearestOpponent();
            ShootBullet(target);
            PlayAnim("battle_attack");
            yield return new WaitForSeconds(0.15f);
        }
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
