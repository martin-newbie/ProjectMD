using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picardy : ActiveSkillBehaviour
{
    public Picardy(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
    {
    }

    public override IEnumerator ActiveSkill(SkillData skillData)
    {
        throw new System.NotImplementedException();
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
        yield return new WaitForSeconds(0.5f);
        var target = GetPreferTarget();
        ShootBullet(target);

        float atkTime = GetAnimTime("battle_attack");
        yield return PlayAnimAndWait("battle_attack", false, atkTime / GetStatus(StatusType.ATK_TIMESCALE));

        yield return new WaitForSeconds(0.1f);
    }
}
