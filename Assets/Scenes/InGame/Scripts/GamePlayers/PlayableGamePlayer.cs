using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayableGamePlayer : GamePlayer
{
    public static PlayableGamePlayer Instance = null;

    SkillCanvas skillCanvas;
    int prevCost = 0;

    int startIdx;
    List<int> chainedList;

    public PlayableGamePlayer(UnitData[] unitDatas, UnitGroupType _group, SkillCanvas _skillCanvas) : base(_group)
    {
        Instance = this;
        skillCanvas = _skillCanvas;

        for (int i = 0; i < unitDatas.Length; i++)
        {
            var data = unitDatas[i];
            var unitStatus = data.GetStatus();
            var unit = InGameManager.Instance.SpawnUnit(new Vector3(-3 - (i * 1.5f), -1, 0), group, data, unitStatus, 0);
            unit.InjectDeadEvent(() => RemoveActiveUnit(unit));
            (unit as SkillBehaviour).InitPlayer(this);
            unit.state = BehaviourState.STANDBY;
            unit.SetActiveHpBar(true);
            AddActiveUnit(unit);
        }
        skillDelay = 2f;
        // skillDelay = 8f;
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
        SetCostUI();
    }

    private void SetCostUI()
    {
        skillCanvas.skillCost.SetCostValueUI(cost);

        int intCost = Mathf.FloorToInt(cost);
        if (prevCost != intCost)
        {
            prevCost = intCost;
            skillCanvas.skillCost.CostRecoverAction(prevCost);
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
        return 10000f; // DEBUG
        return curUnits.Sum((item) => item.GetStatus(StatusType.COST_RECOVERY));
    }

    public override void UseSkill(int idx)
    {
        if (cost < skillDeck[idx].subject.cost)
        {
            return;
        }

        cost -= skillDeck[idx].subject.cost;
        var skillData = skillDeck[idx].subject.GetDefaultSkillValue();
        skillData.collabseCount = skillDeck[idx].chainCount;
        foreach (var item in skillDeck[idx].chained)
        {
            item.CollabseBuff(skillData, skillDeck[idx].subject);
        }
        skillDeck[idx].subject.UseActiveSkill(skillData);
        RemoveSkillAt(idx);
    }

    public void CollabseSkill(int originIdx)
    {
        if (skillDeck[originIdx].locked) return;

        int totalCount = 0;
        int leftIdx = originIdx;
        int rightIdx = originIdx;
        List<DeckSkillData> leftSkills = new List<DeckSkillData>();
        List<DeckSkillData> rightSkills = new List<DeckSkillData>();

        while (totalCount < 5)
        {
            bool leftFinish = false;
            bool rightFinish = false;

            if (leftIdx - 1 >= 0 && skillDeck[originIdx].subject.skillType == skillDeck[leftIdx - 1].subject.skillType && !skillDeck[leftIdx - 1].locked)
            {
                leftIdx--;
                leftSkills.Add(skillDeck[leftIdx]);
                totalCount++;
            }
            else
            {
                leftFinish = true;
            }

            if (rightIdx + 1 < skillDeck.Count && skillDeck[originIdx].subject.skillType == skillDeck[rightIdx + 1].subject.skillType && !skillDeck[rightIdx + 1].locked && totalCount < 3)
            {
                rightIdx++;
                rightSkills.Add(skillDeck[rightIdx]);
                totalCount++;
            }
            else
            {
                rightFinish = true;
            }

            if (leftFinish && rightFinish)
            {
                break;
            }
        }

        if (totalCount == 0) return;

        skillDeck[originIdx].locked = true;
        skillDeck[originIdx].chainCount = totalCount;
        skillDeck[originIdx].chained.AddRange(leftSkills.Select(item => item.subject));
        skillDeck[originIdx].chained.AddRange(rightSkills.Select(item => item.subject));

        if (rightSkills.Count > 0)
            skillDeck.RemoveRange(originIdx + 1, rightSkills.Count);
        if (leftSkills.Count > 0)
            skillDeck.RemoveRange(leftIdx, leftSkills.Count);

        skillCanvas.CollabseEffect(originIdx, leftIdx, leftSkills.Count, originIdx + 1, rightSkills.Count, () =>
        {
            // 체인된 요소를 전부 리스트에서 제외시키기 때문에 가장 왼쪽의 버튼에 아이템에 데이터를 입력한다
            skillCanvas.SetSkillButtonChain(leftIdx, totalCount);
        });
    }
}
