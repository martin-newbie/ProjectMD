using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayableGamePlayer : GamePlayer
{
    public static PlayableGamePlayer Instance = null;

    SkillCanvas skillCanvas;
    int prevCost = 0;

    bool isHolding = false;
    float holdProgress;
    float holdDur = 2f;
    int startIdx;
    int leftLast, rightLast;
    List<int> chainedList;

    public PlayableGamePlayer(UnitData[] unitDatas, UnitGroupType _group, SkillCanvas _skillCanvas) : base(_group)
    {
        Instance = this;
        skillCanvas = _skillCanvas;

        for (int i = 0; i < unitDatas.Length; i++)
        {
            var data = unitDatas[i];
            var unitStatus = data.GetStatus();
            var unit = InGameManager.Instance.SpawnUnit(new Vector3(-3 - (i * 1.5f), 0, 0), group, data, unitStatus, 0);
            unit.InjectDeadEvent(() => RemoveActiveUnit(unit));
            (unit as SkillBehaviour).InitPlayer(this);
            unit.state = BehaviourState.STANDBY;
            unit.SetActiveHpBar(true);
            AddActiveUnit(unit);
        }
        skillDelay = 2f;
        // skillDelay = 8f;
    }

    public override void Update()
    {
        base.Update();

        SkillChainProgress();
    }

    void SkillChainProgress()
    {
        if (!isHolding) return;

        holdProgress += Time.deltaTime;

        bool leftChainAble = leftLast > 0 && startIdx - leftLast < 2 && skillDeck[leftLast].skillType == skillDeck[leftLast - 1].skillType;
        bool rightChainAble = rightLast < skillDeck.Count - 1 && rightLast - startIdx < 2 && skillDeck[rightLast].skillType == skillDeck[rightLast + 1].skillType;

        if (leftChainAble)
        {
            skillCanvas.activatingBtn[leftLast - 1].SetProgress(holdProgress / holdDur);
        }
        if (rightChainAble)
        {
            skillCanvas.activatingBtn[rightLast + 1].SetProgress(holdProgress / holdDur);
        }

        if (holdDur <= holdProgress)
        {
            if (leftChainAble)
            {
                leftLast--;
                skillCanvas.activatingBtn[leftLast].SelectButton();
                chainedList.Add(leftLast);
            }
            if (rightChainAble)
            {
                rightLast++;
                skillCanvas.activatingBtn[rightLast].SelectButton();
                chainedList.Add(rightLast);
            }
            holdProgress = 0f;
        }
    }

    public void StartSkill(int _startIdx)
    {
        isHolding = true;

        startIdx = _startIdx;
        leftLast = startIdx;
        rightLast = startIdx;

        chainedList = new List<int>();

        skillCanvas.activatingBtn[startIdx].SelectButton();
    }

    public void UseSkill()
    {
        if (cost < skillDeck[startIdx].cost)
        {
            ClearChain();
            return;
        }
        if (!skillDeck[startIdx].GetActiveSkillCondition())
        {
            ClearChain();
            return;
        }

        cost -= skillDeck[startIdx].cost;
        isHolding = false;

        var skillData = skillDeck[startIdx].GetDefaultSkillValue();
        foreach (var item in chainedList)
        {
            skillDeck[item].CollabseBuff(skillData, skillDeck[startIdx]);
        }
        skillDeck[startIdx].UseActiveSkill(skillData);
        ClearChain();

        int removeCount = rightLast - leftLast;
        for (int i = 0; i < removeCount + 1; i++)
        {
            RemoveSkillAt(leftLast);
        }
    }

    public void ActiveAllUnits()
    {
        foreach (var unit in curUnits)
        {
            unit.SetBehaviourState(BehaviourState.INCOMBAT);
        }
    }

    public bool EveryUnitActionEnds()
    {
        bool result = true;

        foreach (var item in curUnits)
        {
            if (item.isAction)
            {
                result = false;
                break;
            }
        }

        return result;
    }

    protected override void CostRecoveryLogic()
    {
        base.CostRecoveryLogic();
        SetCostUI(cost / 10);
    }

    private void SetCostUI(float value)
    {
        skillCanvas.skillCost.SetCostValueUI(value);

        int intCost = Mathf.FloorToInt(cost);
        if (prevCost != intCost)
        {
            prevCost = intCost;
            skillCanvas.skillCost.CostRecoverAction(prevCost);
        }
    }

    public override void RemoveActiveUnit(UnitBehaviour removedUnit)
    {
        var startSkill = skillDeck[startIdx];
        if (chainedList.Select(item => skillDeck[item]).Contains(removedUnit))
        {
            // 유닛이 체인 스킬일 경우
            chainedList.RemoveAll(item => skillDeck[item] == removedUnit);
        }
        if (startSkill == removedUnit)
        {
            // 유닛이 시작 스킬일 경우
            isHolding = false;
            ClearChain();
        }

        base.RemoveActiveUnit(removedUnit);
    }

    void ClearChain()
    {
        isHolding = false;
        DeSelectAllButtons();
        chainedList.Clear();
    }

    void DeSelectAllButtons()
    {
        foreach (var item in skillCanvas.activatingBtn)
        {
            item.DeselectButton();
        }
    }

    protected override void RemoveCharacterSkillAt(int idx)
    {
        base.RemoveCharacterSkillAt(idx);
        skillCanvas.RemoveButtonAt(idx);
    }

    protected override SkillBehaviour AddSkillInDeck()
    {
        var data = base.AddSkillInDeck();
        if (data != null)
        {
            skillCanvas.AddSkillButton(data);
        }
        return data;
    }

    protected override void RemoveSkillsRange(int startIdx, int collabseCount)
    {
        base.RemoveSkillsRange(startIdx, collabseCount);
        skillCanvas.RemoveButtonRange(startIdx, collabseCount + 1);
    }

    protected override void RemoveSkillAt(int idx)
    {
        base.RemoveSkillAt(idx);
        skillCanvas.RemoveButtonAt(idx);
    }

    protected override float GetCostRecovery()
    {
        return curUnits.Sum((item) => item.GetStatus(StatusType.COST_RECOVERY));
    }
}
