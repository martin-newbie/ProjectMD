using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yui : ActiveSkillBehaviour
{
    public Yui(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
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
        var target = GetNearestOpponent();
        ShootBullet(target);

        yield return PlayAnimAndWait("battle_attack");
    }

}
