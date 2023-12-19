using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBossGamePlayer : GamePlayer
{
    public TestBossGamePlayer(int[] _unitIdx, int[] _posIdx, UnitGroupType _group) : base(_unitIdx, _posIdx, _group)
    {
    }

    protected override UnitBehaviour SpawnUnit(int idx)
    {
        var obj = InGameManager.Instance.SpawnUnitObject(InGameManager.Instance.posList[posIdx[idx]]);
        var behaviour = new TrainingDummy(obj);
        obj.SetBehaviour(behaviour, 0, group, 1);
        return behaviour;
    }
}
