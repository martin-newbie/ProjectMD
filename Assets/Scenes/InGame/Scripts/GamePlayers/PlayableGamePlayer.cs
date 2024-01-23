using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableGamePlayer : GamePlayer
{
    public static PlayableGamePlayer Instance = null;

    SkillCanvas skillCanvas;
    int prevCost = 0;

    public PlayableGamePlayer(UnitData[] unitDatas, UnitGroupType _group, SkillCanvas _skillCanvas): base(_group)
    {
        skillCanvas = _skillCanvas;
        Instance = this;

        // skillDelay = 1f;
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
}
