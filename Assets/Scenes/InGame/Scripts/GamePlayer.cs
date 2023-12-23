using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayer
{
    public bool isGameActive = false;

    public List<UnitBehaviour> curUnits = new List<UnitBehaviour>();
    public List<List<UnitBehaviour>> allUnits = new List<List<UnitBehaviour>>();
    protected UnitGroupType group;

    // about skill
    List<ActiveSkillBehaviour> skillUnits = new List<ActiveSkillBehaviour>();
    List<ActiveSkillBehaviour> skillDeck = new List<ActiveSkillBehaviour>();
    int[][] unitIdx;
    int[][] posIdx;
    int curShow;

    public float skillCost;
    public float costCharge;

    float skillDelay = 1f;
    float curDelay;

    public GamePlayer(int[][] _unitIdx, int[][] _posIdx, UnitGroupType _group)
    {
        group = _group;

        for (int i = 0; i < _unitIdx.GetLength(0); i++)
        {
            allUnits.Add(new List<UnitBehaviour>());
            for (int j = 0; j < _unitIdx.GetLength(1); j++)
            {
                var unit = SpawnUnit(_unitIdx[i][j], _posIdx[i][j]);
                unit.InjectDeadEvent(() => { RemoveCharacter(unit); });
                unit.gameObject.SetActive(false);

                allUnits[i].Add(unit);
            }
        }

        unitIdx = _unitIdx;
        posIdx = _posIdx;
    }

    public virtual void ShowUnits(int show)
    {
        curShow = show;
        var listUnit = allUnits[curShow];

        for (int i = 0; i < listUnit.Count; i++)
        {
            var unit = listUnit[i];
            unit.gameObject.SetActive(true);
            curUnits.Add(unit);

            if (unit is ActiveSkillBehaviour)
            {
                skillUnits.Add(unit as ActiveSkillBehaviour);
            }
        }
    }

    protected virtual void ClearUnits()
    {
        foreach (var unit in curUnits)
        {
            InGameManager.Instance.allUnits.Remove(unit);
        }
        curUnits.Clear();
        skillUnits.Clear();
        skillDeck.Clear();
    }

    public virtual void RemoveCharacter(UnitBehaviour retiredUnit)
    {
        curUnits.Remove(retiredUnit);
        allUnits[curShow].Remove(retiredUnit);
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
        foreach (var unit in curUnits)
        {
            unit.SetBehaviourState(state);
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

    protected virtual UnitBehaviour SpawnUnit(int unitIdx, int posIdx)
    {
        return InGameManager.Instance.SpawnUnit(InGameManager.Instance.posList[posIdx], unitIdx, group, 0);
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
        for (int i = 0; i < curUnits.Count; i++)
        {
            var unit = curUnits[i];
            InGameManager.Instance.StartCoroutine(unit.CommonMoveToPosEndWait(InGameManager.Instance.posList[i]));
        }
    }

    public virtual void OnEnd()
    {
        isGameActive = false;
    }
}
