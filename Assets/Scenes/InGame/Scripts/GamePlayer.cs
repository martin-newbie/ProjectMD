using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayer
{
    public bool isGameActive = false;

    public List<UnitBehaviour> playerUnits = new List<UnitBehaviour>();
    protected int[] unitIdx;
    protected int[] posIdx;
    protected UnitGroupType group;

    // about skill
    List<ActiveSkillBehaviour> skillUnits = new List<ActiveSkillBehaviour>();
    List<ActiveSkillBehaviour> skillDeck = new List<ActiveSkillBehaviour>();

    public float skillCost;
    public float costCharge;

    float skillDelay = 1f;
    float curDelay;

    public GamePlayer(int[] _unitIdx, int[] _posIdx, UnitGroupType _group)
    {
        unitIdx = _unitIdx;
        posIdx = _posIdx;
        group = _group;
    }

    public virtual void SpawnCharacter()
    {
        for (int i = 0; i < unitIdx.Length; i++)
        {
            var unit = SpawnUnit(i);
            unit.InjectDeadEvent(() => { RemoveCharacter(unit); });

            playerUnits.Add(unit);
            InGameManager.Instance.allUnits.Add(unit);
            if (unit is ActiveSkillBehaviour)
            {
                skillUnits.Add(unit as ActiveSkillBehaviour);
            }
        }

        isGameActive = true;
    }

    public virtual void RemoveCharacter(UnitBehaviour retiredUnit)
    {
        playerUnits.Remove(retiredUnit);
        InGameManager.Instance.allUnits.Remove(retiredUnit);

        if (retiredUnit is ActiveSkillBehaviour)
        {
            skillUnits.Remove(retiredUnit as ActiveSkillBehaviour);
            for (int i = 0; i < skillDeck.Count; i++)
            {
                if (skillDeck[i] == retiredUnit)
                {
                    RemoveCharacterSkillAt(i);
                    i--;
                }
            }
        }

        InGameManager.Instance.StartCoroutine(disappearObject(retiredUnit));

        IEnumerator disappearObject(UnitBehaviour unit)
        {
            yield return new WaitForSeconds(2f);
            unit.gameObject.SetActive(false);
        }
    }

    public virtual void SetUnitsState(BehaviourState state)
    {
        foreach (var unit in playerUnits)
        {
            unit.SetBehaviourState(state);
        }
    }

    protected virtual void RemoveCharacterSkillAt(int idx)
    {
        skillDeck.RemoveAt(idx);
    }

    protected virtual UnitBehaviour SpawnUnit(int idx)
    {
        return InGameManager.Instance.SpawnUnit(InGameManager.Instance.posList[posIdx[idx]], unitIdx[idx], group, 0);
    }

    public virtual void Update()
    {
        if (!isGameActive) return;

        if (curDelay >= skillDelay)
        {
            curDelay = 0;
            AddSkillInDeck();
        }

        skillCost += Time.deltaTime * costCharge;
        curDelay += Time.deltaTime;
    }

    protected virtual ActiveSkillBehaviour AddSkillInDeck()
    {
        if (skillDeck.Count >= 10) return null;

        int rand = Random.Range(0, skillUnits.Count);
        var skillData = skillUnits[rand];

        skillDeck.Add(skillData);
        return skillData;
    }

    public virtual void UseSkill(int idx)
    {
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

        var skillValue = skillDeck[idx].GetDefaultSkillValue();
        foreach (var skill in collabse)
        {
            skill.CollabseSkill(skillValue, skillDeck[idx]);
        }
        skillDeck[idx].UseActiveSkill(skillValue);


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

    protected virtual void RemoveSkillsRange(int startIdx, int collabseCount)
    {
        skillDeck.RemoveRange(startIdx, collabseCount + 1);
    }

    protected virtual void RemoveSkillAt(int idx)
    {
        skillDeck.RemoveAt(idx);
    }

    public virtual void ReturnOriginPos()
    {
        for (int i = 0; i < unitIdx.Length; i++)
        {
            var unit = playerUnits.Find((item) => item.keyIndex == unitIdx[i]);
            if (unit != null)
            {
                unit.CommonMoveToPosition(InGameManager.Instance.posList[posIdx[i]]);
            }
        }
    }

    public virtual void OnEnd()
    {
        isGameActive = false;
    }
}
