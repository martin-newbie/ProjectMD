using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveAbleBehaviour : PassiveAbleBehaviour
{
    protected ActiveAbleBehaviour(UnitObject _subject) : base(_subject)
    {
        // get active skill value
    }

    public void ActivateActiveSkill(ActiveSkillValue skillData)
    {
        StartActionCoroutine(ActiveSkill(skillData));
    }

    public abstract void CollabseSkill(ActiveSkillValue skillData, UnitBehaviour subjectUnit);
    public abstract IEnumerator ActiveSkill(ActiveSkillValue skillData);
}


public class ActiveSkillValue
{
    public Dictionary<StatusType, float> valueDic = new Dictionary<StatusType, float>();

    public ActiveSkillValue()
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