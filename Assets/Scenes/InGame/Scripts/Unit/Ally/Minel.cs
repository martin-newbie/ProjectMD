using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minel : ActiveSkillBehaviour, IEventPost
{

    public Minel(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
    {
        InGameEvent.Add(EventType.HIT_CRITICAL, this);
    }

    protected override void InCombatFunc()
    {
        base.InCombatFunc();
    }

    protected override IEnumerator AttackLogic()
    {
        yield return StartCoroutine(CommonBurstFire(3));
    }

    public override IEnumerator ActiveSkill(SkillData skillData)
    {
        yield return PlayAnimAndWait("battle_reload");
        yield break;
    }

    public override void CollabseBuff(SkillData skillData, UnitBehaviour subjectUnit)
    {
    }

    public override bool ActiveSkillCondition()
    {
        return false;
    }

    public void PostEvent(EventType type, UnitBehaviour from, UnitBehaviour to)
    {
        if (type == EventType.HIT_CRITICAL && from == this)
        {
            AddBuff(StatusType.ATK_DELAY, skillStatus.GetActiveSkillValue(unitData.skill_level[1]), 15);
        }
    }
}
