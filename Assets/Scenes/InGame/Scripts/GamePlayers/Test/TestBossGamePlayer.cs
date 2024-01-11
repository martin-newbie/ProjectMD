using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBossGamePlayer : GamePlayer
{
    public int stageLevel;

    public TestBossGamePlayer(DeckData[] _unitIdx, Vector3[][] _posIdx, UnitGroupType _group, int _stageLevel) : base(_unitIdx, _posIdx, _group)
    {
        stageLevel = _stageLevel;
    }

    protected override UnitBehaviour SpawnUnit(int unitIdx, Vector3 pos)
    {
        var obj = InGameManager.Instance.SpawnUnitObject(pos);
        var behaviour = new TrainingDummy(obj);
        obj.SetBehaviour(behaviour, 0, group, stageLevel, 1);
        return behaviour;
    }
}
