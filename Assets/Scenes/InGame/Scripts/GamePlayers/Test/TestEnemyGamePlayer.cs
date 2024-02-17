using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyGamePlayer : GamePlayer
{
    int stageLevel;
    List<List<UnitBehaviour>> waveList = new List<List<UnitBehaviour>>();

    public TestEnemyGamePlayer(UnitData[][] unitDatas, UnitGroupType _group, int _stageLevel) : base(_group)
    {
        waveList = new List<List<UnitBehaviour>>();
        stageLevel = _stageLevel;
        InitUnits(unitDatas, _group);
    }

    void InitUnits(UnitData[][] unitDatas, UnitGroupType group)
    {
        for (int i = 0; i < unitDatas.Length; i++)
        {
            waveList.Add(new List<UnitBehaviour>());

            var waveData = unitDatas[i];
            float range = 6 / waveData.Length;

            for (int j = 0; j < waveData.Length; j++)
            {
                var data = waveData[j];
                var spawnPos = new Vector3(i * 10 + 3 + range * j, 0, 0);
                var statusData = StaticDataManager.GetUnitStatus(data.index).GetCalculatedValueDictionary();

                int barType = 0;
                if (i == unitDatas.Length - 1 && j == waveData.Length - 1)
                {
                    statusData[StatusType.DMG] /= 2;
                    statusData[StatusType.HP] *= 2;
                    barType = 1;
                }
                else
                {
                    statusData[StatusType.DMG] /= 3;
                    statusData[StatusType.HP] /= 3;
                }

                var unit = InGameManager.Instance.SpawnUnit(spawnPos, group, data, statusData, barType);
                unit.state = BehaviourState.STANDBY;
                unit.InjectDeadEvent(() => RemoveActiveUnit(unit));
                unit.SetActiveHpBar(false);
                waveList[i].Add(unit);
            }
        }
    }

    public void ActiveUnitAt(int idx)
    {
        var units = waveList[idx];
        foreach (var unit in units)
        {
            unit.SetBehaviourState(BehaviourState.INCOMBAT);
            AddActiveUnit(unit);
        }
    }
}
