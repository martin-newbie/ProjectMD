using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luria : ActiveSkillBehaviour
{
    public Luria(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
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

    protected override IEnumerator MoveToTargetRange()
    {
        PlayAnim("battle_move", true);
        yield return StartCoroutine(CombatMoveLogic());
        PlayAnim("battle_move_stop", true);
        AddAnim("battle_wait", true);
    }
}
