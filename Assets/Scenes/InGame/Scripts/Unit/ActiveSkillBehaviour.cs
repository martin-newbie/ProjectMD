using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveSkillBehaviour : UnitBehaviour
{
    public AttributeType skillType;
    public UnitSkillStatus skillStatus;
    public GamePlayer player;
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

    public virtual void InitPlayer(GamePlayer _player)
    {
        player = _player;
    }

    public virtual Coroutine UseActiveSkill(SkillData skillData)
    {
        isAction = true;

        StopAllCoroutine();
        var coroutine = StartCoroutine(StartActiveSkillRoutine(ActiveSkill(skillData)));
        return coroutine;
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
    public virtual bool GetActiveSkillCondition()
    {
        return state != BehaviourState.ACTIVE_SKILL && state != BehaviourState.RETIRE && player.cost >= skillStatus.cost && !isCC;
    }
}

public class SkillData
{
    public int collabseCount;
    public DamageStruct damageData;
}