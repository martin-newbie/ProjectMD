using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBossGamePlayer : GamePlayer
{
    public int stageLevel;
    UnitBehaviour bossUnit;

    public TestBossGamePlayer(UnitData unitData, UnitGroupType _group, int _stageLevel) : base(_group)
    {
        stageLevel = _stageLevel;

        InitUnit(unitData);
    }

    void InitUnit(UnitData data)
    {
        var statusData = StaticDataManager.GetUnitStatus(data.index).GetCalculatedValueDictionary(data.level);
        var spawnPos = new Vector3(5, 0, 0);
        int barType = 1;
        statusData[StatusType.DMG] /= 3;
        statusData[StatusType.HP] *= 3;
        bossUnit = InGameManager.Instance.SpawnUnit(spawnPos, group, data, statusData, barType);
        AddActiveUnit(bossUnit);
        bossUnit.InjectDeadEvent(() => RemoveActiveUnit(bossUnit));
    }

    public override void StartGame()
    {
        base.StartGame();
        bossUnit.UnitActive();
    }
}
