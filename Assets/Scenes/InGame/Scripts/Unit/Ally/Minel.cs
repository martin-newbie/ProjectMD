using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minel : ActiveSkillBehaviour, IEventPost
{

    public Minel(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
    {
        InGameEvent.Add(EventType.HIT_CRITICAL, this);
        InGameEvent.Add(EventType.MISS_ATTACK, this);
    }

    public override void UnitActive()
    {
        base.UnitActive();
        AddBuff(StatusType.DEF, skillStatus.GetEnforceSkillValue(unitData.skill_level[2]), 0);
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
        AddBuff(StatusType.ATK_TIMESCALE, skillStatus.GetActiveSkillValue(unitData.skill_level[0]), 15);
        curAmmo = maxAmmo; // 즉시 재장전 연출 필요
        yield break;
    }

    public override void CollabseBuff(SkillData skillData, UnitBehaviour subjectUnit)
    {
        skillData.damageData.AddIncreaseValue(StatusType.CRI_DAMAGE, 24f);
    }

    public override bool GetActiveSkillCondition()
    {
        return base.GetActiveSkillCondition();
    }

    public void PostEvent(EventType type, UnitBehaviour from, UnitBehaviour to)
    {
        if (type == EventType.HIT_CRITICAL && from == this)
        {
            AddBuff(StatusType.DMG, skillStatus.GetPassiveSkillValue(unitData.skill_level[1]), 15);
            to.GetTaunted(this, 15);
        }
        if(type == EventType.MISS_ATTACK && from == this)
        {
            // 공격력 상승인지 방어력 상승인지 확인되면 적용
        }
    }
}
