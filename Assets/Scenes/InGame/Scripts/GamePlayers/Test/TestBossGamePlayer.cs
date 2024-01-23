using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBossGamePlayer : GamePlayer
{
    public int stageLevel;

    public TestBossGamePlayer(UnitData[] unitDatas, UnitGroupType _group, int _stageLevel) : base(_group)
    {
        stageLevel = _stageLevel;
    }
}
