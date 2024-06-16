using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEnemyPlayer : GamePlayer
{
    int stageLevel;
    List<List<UnitBehaviour>> waveUnits;

    public StageEnemyPlayer(UnitGroupType _group, StageData stageData) : base(_group)
    {
        waveUnits = new List<List<UnitBehaviour>>();

        stageLevel = stageData.stageLevel;
        float spawnX = 3f;
        foreach (var waveData in stageData.waveDatas)
        {
            spawnX += 10f;
            var unitList = new List<UnitBehaviour>();
            float dist = 6f / waveData.unitDatas.Count;
            foreach (var unitData in waveData.unitDatas)
            {
                var unitStatus = unitData.GetStatus();
                switch (unitData.unit_type)
                {
                    case 0:
                        unitStatus[StatusType.DMG] /= 2f;
                        unitStatus[StatusType.HP] /= 3f;
                        unitStatus[StatusType.DEF] /= 2f;
                        break;
                    case 1:
                        unitStatus[StatusType.HP] /= 2f;
                        unitStatus[StatusType.DEF] /= 2f;
                        break;
                    case 2:
                        unitStatus[StatusType.DMG] /= 2f;
                        unitStatus[StatusType.HP] *= 3f;
                        break;
                }

                var unit = InGameManager.Instance.SpawnUnit(new Vector3(spawnX, -1), _group, unitData, unitStatus, unitData.unit_type);
                unit.state = BehaviourState.STANDBY;
                unit.InjectDeadEvent(() => RemoveActiveUnit(unit));
                unit.SetActiveHpBar(false);
                unitList.Add(unit);
                spawnX += dist;
            }
            waveUnits.Add(unitList);
        }
    }

    public void ActiveUnitAt(int idx)
    {
        var units = waveUnits[idx];
        foreach (var unit in units)
        {
            unit.SetBehaviourState(BehaviourState.INCOMBAT);
            AddActiveUnit(unit);
        }
    }
}
