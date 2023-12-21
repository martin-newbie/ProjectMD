using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyGamePlayer : GamePlayer
{
    public TestEnemyGamePlayer(int[] _unitIdx, int[] _posIdx, UnitGroupType _group) : base(_unitIdx, _posIdx, _group)
    {

    }

    protected override UnitBehaviour SpawnUnit(int idx)
    {
        var unit = base.SpawnUnit(idx);
        InGameManager.Instance.movingObjects.Add(unit.gameObject);
        return unit;
    }
}
