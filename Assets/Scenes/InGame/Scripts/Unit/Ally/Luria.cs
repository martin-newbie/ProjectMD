using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luria : ActiveSkillBehaviour
{
    public Luria(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
    {
    }

    public override IEnumerator ActiveSkill(SkillData skillData)
    {
        yield break;
    }

    public override bool GetActiveSkillCondition()
    {
        return false;
    }

    public override void CollabseBuff(SkillData skillData, UnitBehaviour subjectUnit)
    {
    }

    protected override IEnumerator AttackLogic()
    {
        for (int i = 0; i < 3; i++)
        {
            var target = GetPreferTarget();
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
