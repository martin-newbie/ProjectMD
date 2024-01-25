using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GamePlayer
{
    public bool isGameActive = false;

    public List<UnitBehaviour> curUnits = new List<UnitBehaviour>();
    protected UnitGroupType group;

    // about skill
    List<ActiveSkillBehaviour> skillUnits = new List<ActiveSkillBehaviour>();
    List<ActiveSkillBehaviour> skillDeck = new List<ActiveSkillBehaviour>();

    public float cost;

    protected float skillDelay = 8f;
    protected float curDelay;

    public GamePlayer(UnitGroupType _group)
    {
        group = _group;
    }

    public virtual void AddActiveUnit(UnitBehaviour addedUnit)
    {
        curUnits.Add(addedUnit);
        InGameManager.Instance.allUnits.Add(addedUnit);

        if (addedUnit is ActiveSkillBehaviour)
        {
            skillUnits.Add(addedUnit as ActiveSkillBehaviour);
        }
    }

    public virtual void RemoveActiveUnit(UnitBehaviour removedUnit)
    {
        curUnits.Remove(removedUnit);
        InGameManager.Instance.allUnits.Remove(removedUnit);

        if (removedUnit is ActiveSkillBehaviour)
        {
            skillUnits.Remove(removedUnit as ActiveSkillBehaviour);
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
            item.PlayAnim(key, loop);
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

        if (cost <= 10f)
        {
            CostRecoveryLogic();
        }
    }

    protected virtual ActiveSkillBehaviour AddSkillInDeck()
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

        int collabseCount = 0;
        int left = idx - 1;
        int right = idx + 1;
        List<ActiveSkillBehaviour> collabse = new List<ActiveSkillBehaviour>();
        bool leftFinish = false, rightFinish = false;
        int startIdx = idx;

        while (!leftFinish || !rightFinish)
        {
            if (left >= 0 && collabseCount < 4 && skillDeck[left].skillType == skillDeck[idx].skillType)
            {
                collabse.Add(skillDeck[left]);
                startIdx = left;
                left = left - 1;
                collabseCount++;
            }
            else
            {
                leftFinish = true;
            }

            if (right < skillDeck.Count && collabseCount < 4 && skillDeck[right].skillType == skillDeck[idx].skillType)
            {
                collabse.Add(skillDeck[right]);
                right = right + 1;
                collabseCount++;
            }
            else
            {
                rightFinish = true;
            }
        }

        var skillData = skillDeck[idx].GetDefaultSkillValue();
        skillData.collabseCount = collabseCount;
        foreach (var skill in collabse)
        {
            skill.CollabseBuff(skillData, skillDeck[idx]);
        }
        skillDeck[idx].UseActiveSkill(skillData);
        cost -= skillDeck[idx].cost;

        // remove collabse skills
        if (collabseCount > 0)
        {
            RemoveSkillsRange(startIdx, collabseCount);
        }
        else
        {
            RemoveSkillAt(idx);
        }

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
        curDelay += Time.deltaTime;
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
