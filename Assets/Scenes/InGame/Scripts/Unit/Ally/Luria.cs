using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luria : SkillBehaviour
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
            float atkTime = GetAnimTime("battle_attack");
            PlayAnimAndWait("battle_attack", false, atkTime / GetStatus(StatusType.ATK_TIMESCALE));

            ShootBullet(target, "bullet_pos1");
            yield return new WaitForSeconds(0.07f);
            ShootBullet(target, "bullet_pos2");
            yield return new WaitForSeconds(0.07f);
        }
    }

    protected override IEnumerator MoveToTargetRange()
    {
        PlayAnimAndWait("battle_move", true);
        yield return StartCoroutine(CombatMoveLogic());
        PlayAnimAndWait("battle_move_stop", true);
        AddAnim("battle_wait", true);
    }
}
