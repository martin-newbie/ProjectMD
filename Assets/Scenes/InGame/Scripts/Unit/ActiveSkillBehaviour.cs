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

    public virtual Coroutine UseActiveSkill(SkillData skillData)
    {
        isAction = true;

        if (actionCoroutine != null) StopCoroutine(actionCoroutine);
        actionCoroutine = StartCoroutine(StartActiveSkillRoutine(ActiveSkill(skillData)));
        return actionCoroutine;
    }

    protected virtual IEnumerator StartActiveSkillRoutine(IEnumerator routine)
    {
        state = BehaviourState.ACTIVE_SKILL;
        yield return StartCoroutine(routine);
        state = BehaviourState.INCOMBAT;
    }

    public virtual SkillData GetDefaultSkillValue()
    {
        SkillData result = new SkillData();
        result.damageData = GetDamageStruct();
        return result;
    }

    public abstract void CollabseBuff(SkillData skillData, UnitBehaviour subjectUnit);
    public abstract IEnumerator ActiveSkill(SkillData skillData);
    public abstract bool ActiveSkillCondition();
}

public class SkillData
{
    public int collabseCount;
    public DamageStruct damageData;
}