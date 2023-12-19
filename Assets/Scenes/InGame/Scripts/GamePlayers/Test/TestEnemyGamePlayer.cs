using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyGamePlayer : GamePlayer
{
    public TestEnemyGamePlayer(int[] _unitIdx, int[] _posIdx, UnitGroupType _group) : base(_unitIdx, _posIdx, _group)
    {
        for (int i = 0; i < posIdx.Length; i++)
        {
            posIdx[i] += 20;
        }
    }

    protected override UnitBehaviour SpawnUnit(int idx)
    {
        var unit = base.SpawnUnit(idx);
        InGameManager.Instance.movingObjects.Add(unit.gameObject);
        return unit;
    }
}
