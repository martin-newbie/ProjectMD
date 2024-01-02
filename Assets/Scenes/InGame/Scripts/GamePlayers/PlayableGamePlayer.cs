using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableGamePlayer : GamePlayer
{
    public static PlayableGamePlayer Instance = null;

    SkillCanvas skillCanvas;

    public PlayableGamePlayer(int[][] _unitIdx, Vector3[][] _posIdx, UnitGroupType _group, SkillCanvas _skillCanvas) : base(_unitIdx, _posIdx, _group)
    {
        skillCanvas = _skillCanvas;
        Instance = this;
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
