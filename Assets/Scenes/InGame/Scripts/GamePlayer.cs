using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GamePlayer
{
    public bool isGameActive = false;

    public List<UnitBehaviour> curUnits = new List<UnitBehaviour>();
    public List<List<UnitBehaviour>> allUnits = new List<List<UnitBehaviour>>();
    protected UnitGroupType group;

    // about skill
    List<ActiveSkillBehaviour> skillUnits = new List<ActiveSkillBehaviour>();
    List<ActiveSkillBehaviour> skillDeck = new List<ActiveSkillBehaviour>();
    DeckData[] unitIdx;
    Vector3[][] posArr;

    public int curShow;
    public float skillCost;
    public float costCharge;

    float skillDelay = 1f;
    float curDelay;

    public GamePlayer(DeckData[] _unitIdx, Vector3[][] _posArr, UnitGroupType _group)
    {
        group = _group;
        unitIdx = _unitIdx;
        posArr = _posArr;

        for (int i = 0; i < unitIdx.Length; i++)
        {
            allUnits.Add(new List<UnitBehaviour>());
            var units = unitIdx[i];
            for (int j = 0; j < units.unitsIdx.Length; j++)
            {
                var unit = SpawnUnit(units.unitsIdx[j], posArr[i][j]);
                unit.InjectDeadEvent(() => { RemoveActiveUnit(unit); });
                unit.DeactiveUnit();
                unit.gameObject.SetActive(false);
                allUnits[i].Add(unit);
            }
        }
    }

    public virtual void ShowUnits(int show)
    {
        if (curShow != show)
        {
            var prevListUnit = allUnits[curShow];
            for (int i = 0; i < prevListUnit.Count; i++)
            {
                var unit = prevListUnit[i];
                if (unit.state != BehaviourState.RETIRE)
                    RemoveActiveUnit(unit);
            }
        }
        curShow = show;
        var listUnit = allUnits[curShow];


        var posArr = this.posArr[curShow];
        for (int i = 0; i < listUnit.Count; i++)
        {
            var unit = listUnit[i];
            unit.transform.position = posArr[i];
            AddActiveUnit(unit);
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

    public virtual void AddActiveUnit(UnitBehaviour addedUnit)
    {
        if (addedUnit.state == BehaviourState.RETIRE)
        {
            return;
        }

        addedUnit.ActiveUnit();
        curUnits.Add(addedUnit);
        InGameManager.Instance.allUnits.Add(addedUnit);
        if (addedUnit is ActiveSkillBehaviour)
        {
            skillUnits.Add(addedUnit as ActiveSkillBehaviour);
        }
    }

    public virtual void RemoveActiveUnit(UnitBehaviour removedUnit)
    {
        removedUnit.DeactiveUnit();
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

        InGameManager.Instance.StartCoroutine(disappearObject(removedUnit));

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

    public void AllUnitsMoveFront()
    {
        foreach (var item in curUnits)
        {
            item.PlayAnim("battle_move", true);
        }
    }

    protected virtual UnitBehaviour SpawnUnit(int unitIdx, Vector3 pos)
    {
        return InGameManager.Instance.SpawnUnit(pos, unitIdx, group, 0);
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

        skillCost += Time.deltaTime * costCharge;
        curDelay += Time.deltaTime;
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
            InGameManager.Instance.StartCoroutine(unit.CommonMoveToPosEndWait(posArr[curShow][i]));
        }
    }

    public virtual void OnEnd()
    {
        isGameActive = false;
    }
}
