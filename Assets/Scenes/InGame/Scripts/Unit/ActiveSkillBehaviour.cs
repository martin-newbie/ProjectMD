using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveSkillBehaviour : UnitBehaviour
{
    public AttributeType skillType;
    public UnitSkillStatus skillStatus;
    public int cost;

    protected ActiveSkillBehaviour(UnitObject _subject) : base(_subject)
    {
    }

    public override void InitCommon(int idx, int level, int barType)
    {
        base.InitCommon(idx, level, barType);
        skillStatus = StaticDataManager.GetUnitSkillStatus(keyIndex);
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