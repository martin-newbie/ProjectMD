using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveSkillBehaviour : UnitBehaviour
{
    public AttributeType skillType;
    public UnitSkillStatus skillStatus;
    public int cost;

    protected ActiveSkillBehaviour(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
    {
    }

    public override void InitObject(UnitObject _subject, int barType)
    {
        base.InitObject(_subject, barType);
        skillStatus = StaticDataManager.GetUnitSkillStatus(constData.keyIndex);
        cost = skillStatus.cost;
    }

    public virtual void UseActiveSkill(DamageStruct skillData)
    {
        StartActionCoroutine(ActiveSkill(skillData));
    }

    public virtual DamageStruct GetDefaultSkillValue()
    {
        DamageStruct result = GetDamageStruct();
        return result;
    }

    public abstract void CollabseBuff(DamageStruct skillData, UnitBehaviour subjectUnit);
    public abstract IEnumerator ActiveSkill(DamageStruct skillData);
    public abstract bool ActiveSkillCondition();
}