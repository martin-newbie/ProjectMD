using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GamePlayer
{
    public bool isGameActive = false;

    public List<UnitBehaviour> curUnits = new List<UnitBehaviour>();
    protected UnitGroupType group;

    // about skill
    List<SkillBehaviour> skillUnits = new List<SkillBehaviour>();
    List<SkillBehaviour> skillDeck = new List<SkillBehaviour>();

    public float cost;

    protected float skillDelay = 8f;
    protected float curDelay;

    public GamePlayer(UnitGroupType _group)
    {
        group = _group;
    }

    public virtual void AddActiveUnit(UnitBehaviour unit)
    {
        unit.UnitActive();
        curUnits.Add(unit);
        InGameManager.Instance.allUnits.Add(unit);

        if (unit is SkillBehaviour)
        {
            skillUnits.Add(unit as SkillBehaviour);
        }
    }

    public virtual void RemoveActiveUnit(UnitBehaviour removedUnit)
    {
        curUnits.Remove(removedUnit);
        InGameManager.Instance.allUnits.Remove(removedUnit);

        if (removedUnit is SkillBehaviour)
        {
            skillUnits.Remove(removedUnit as SkillBehaviour);
            for (int i = 0; i < skillDeck.Count; i++)
            {
                if (skillDeck[i] == removedUnit)
                {
                    RemoveCharacterSkillAt(i);
                    i--;
                }
            }
        }
    }

    public virtual int GetCountOfUnits()
    {
        return curUnits.Count;
    }

    protected virtual void RemoveCharacterSkillAt(int idx)
    {
        skillDeck.RemoveAt(idx);
    }

    public void AllUnitsPlayAnim(string key, bool loop = false)
    {
        foreach (var item in curUnits)
        {
            item.PlayAnimAndWait(key, loop);
        }
    }

    public virtual void StartGame()
    {
        isGameActive = true;
    }

    public virtual void Update()
    {
        if (!isGameActive) return;

        if (curDelay >= skillDelay)
        {
            curDelay = 0;
            AddSkillInDeck();
        }
        curDelay += Time.deltaTime;


        if (cost <= 10f)
        {
            CostRecoveryLogic();
        }
    }

    protected virtual SkillBehaviour AddSkillInDeck()
    {
        if (skillDeck.Count >= 10) return null;
        if (skillUnits.Count <= 0) return null;

        int rand = Random.Range(0, skillUnits.Count);
        var skillData = skillUnits[rand];

        skillDeck.Add(skillData);
        return skillData;
    }

    public virtual void UseSkill(int idx)
    {
        if(cost < skillDeck[idx].cost)
        {
            return;
        }

        RemoveSkillAt(idx);
    }

    protected virtual void CostRecoveryLogic()
    {
        if (cost >= 10f)
        {
            cost = 10f;
        }
        else
        {
            cost += GetCostRecovery() * Time.deltaTime / 10000f;
        }
    }

    protected virtual float GetCostRecovery()
    {
        float value = 0f;
        if (curUnits.Count > 0)
        {
            foreach (var item in curUnits)
            {
                value += item.GetStatus(StatusType.COST_RECOVERY);
            }
        }
        return value;
    }

    protected virtual void RemoveSkillsRange(int startIdx, int collabseCount)
    {
        skillDeck.RemoveRange(startIdx, collabseCount + 1);
    }

    protected virtual void RemoveSkillAt(int idx)
    {
        skillDeck.RemoveAt(idx);
    }

    public virtual void OnEnd()
    {
        isGameActive = false;
        curDelay = 0f;
    }
}
