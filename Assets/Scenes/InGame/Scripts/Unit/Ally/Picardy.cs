using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picardy : ActiveSkillBehaviour
{
    public Picardy(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
    {
    }

    public override IEnumerator ActiveSkill(DamageStruct skillData)
    {
        throw new System.NotImplementedException();
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
        var target = GetNearestOpponent();
        ShootBullet(target);

        yield return PlayAnimAndWait("battle_attack");
    }
}
