using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luria : ActiveSkillBehaviour
{
    public Luria(UnitObject _subject) : base(_subject)
    {
        maxAmmo = 30;
        curAmmo = maxAmmo;
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
        for (int i = 0; i < 3; i++)
        {
            var target = GetNearestOpponent();
            PlayAnim("battle_attack");

            ShootBullet(target, "bullet_pos1");
            yield return new WaitForSeconds(0.07f);
            ShootBullet(target, "bullet_pos2");
            yield return new WaitForSeconds(0.07f);
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

    protected override IEnumerator MoveToTargetRange()
    {
        PlayAnim("battle_move", true);
        yield return StartCoroutine(CombatMoveLogic());
        PlayAnim("battle_move_stop", true);
        AddAnim("battle_wait", true);
    }
}
