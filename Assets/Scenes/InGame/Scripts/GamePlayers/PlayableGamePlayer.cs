using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableGamePlayer : GamePlayer
{
    public static PlayableGamePlayer Instance = null;

    SkillCanvas skillCanvas;
    int prevCost = 0;

    public PlayableGamePlayer(UnitData[] unitDatas, UnitGroupType _group, SkillCanvas _skillCanvas) : base(_group)
    {
        Instance = this;
        skillCanvas = _skillCanvas;

        for (int i = 0; i < unitDatas.Length; i++)
        {
            var data = unitDatas[i];
            var unitStatus = StaticDataManager.GetUnitStatus(data.index).GetCalculatedValueDictionary();
            var unit = InGameManager.Instance.SpawnUnit(new Vector3(-3 - (i * 1.5f), 0, 0), group, data, unitStatus, 0);
            unit.InjectDeadEvent(() => RemoveActiveUnit(unit));
            unit.state = BehaviourState.STANDBY;
            AddActiveUnit(unit);
        }
        skillDelay = 1f;
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

    protected override void RemoveCharacterSkillAt(int idx)
    {
        base.RemoveCharacterSkillAt(idx);
        skillCanvas.RemoveButtonAt(idx);
    }

    protected override ActiveSkillBehaviour AddSkillInDeck()
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

    // test
    protected override float GetCostRecovery()
    {
        return 100000f;
    }
}
