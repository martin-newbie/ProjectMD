using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveSkillBehaviour : UnitBehaviour
{
    public AttributeType skillType;
    public int cost;

    protected ActiveSkillBehaviour(UnitObject _subject) : base(_subject)
    {
        // get active skill value
    }

    public virtual void UseActiveSkill(DamageStruct skillData)
    {
        StartActionCoroutine(ActiveSkill(skillData));
    }

    public virtual DamageStruct GetDefaultSkillValue()
    {
        DamageStruct result = new DamageStruct();
        return result;
    }

    public abstract void CollabseBuff(DamageStruct skillData, UnitBehaviour subjectUnit);
    public abstract IEnumerator ActiveSkill(DamageStruct skillData);
    public abstract bool ActiveSkillCondition();
}