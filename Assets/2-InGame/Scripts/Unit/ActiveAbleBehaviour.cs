using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveAbleBehaviour : PassiveAbleBehaviour
{
    protected ActiveAbleBehaviour(UnitObject _subject) : base(_subject)
    {
        // get active skill value
    }

    public void ActivateActiveSkill(SkillValue skillData)
    {
        StartActionCoroutine(ActiveSkill(skillData));
    }

    public abstract void CollabseSkill(SkillValue skillData, UnitBehaviour subjectUnit);
    public abstract IEnumerator ActiveSkill(SkillValue skillData);
}


public class SkillValue
{
    public Dictionary<StatusType, float> valueDic = new Dictionary<StatusType, float>();

    public SkillValue()
    {
        valueDic = new Dictionary<StatusType, float>();
    }

    public void AddValue(StatusType type, float value)
    {
        if (valueDic.ContainsKey(type))
        {
            valueDic[type] += value;
        }
        else
        {
            valueDic.Add(type, value);
        }
    }

    public float GetAttribute(StatusType type)
    {
        if (valueDic.ContainsKey(type))
        {
            return valueDic[type];
        }
        else
        {
            return 0f;
        }
    }
}