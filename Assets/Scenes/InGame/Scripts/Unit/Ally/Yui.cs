using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yui : SkillBehaviour
{
    public Yui(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
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
        var target = GetPreferTarget();
        ShootBullet(target);

        yield return PlayAnimAndWait("battle_attack");
    }

}
