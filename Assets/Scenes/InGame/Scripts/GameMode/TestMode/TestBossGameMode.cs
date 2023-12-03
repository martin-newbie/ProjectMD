using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBossGameMode : IGameModeBehaviour
{
    int[] spawnIdx = new int[5] { 21, 24, 27, 30, 33 };

    InGameManager manager;

    public TestBossGameMode(InGameManager _manager)
    {
        manager = _manager;

    }

    public IEnumerator GameModeRoutine()
    {
        SpawnAllyUnits();

        SetUnitsState(BehaviourState.INCOMBAT, UnitGroupType.ALLY);
        SpawnEnemy();
        yield return new WaitUntil(() => GetCountOfEnemy() <= 0);
    }

    void SpawnAllyUnits()
    {
        int[] units = TempData.Instance.curDeckUnits;
        for (int i = 0; i < units.Length; i++)
        {
            var unitObj = Object.Instantiate(manager.unitObjPrefab, manager.posList[spawnIdx[i]], Quaternion.identity);
            var behaviour = manager.SetBehaviourInObject(unitObj, units[i], UnitGroupType.ALLY, 0);
            manager.allUnits.Add(behaviour);
        }

        manager.InitSkill();
    }

    void SetUnitsState(BehaviourState state, UnitGroupType group)
    {
        foreach (var unit in manager.allUnits)
        {
            if (unit.group == group)
            {
                unit.SetBehaviourState(state);
            }
        }
    }

    void SpawnEnemy()
    {
        var unitObj = Object.Instantiate(manager.unitObjPrefab, manager.posList[50], Quaternion.identity);
        manager.movingObjects.Add(unitObj.gameObject);

        var behaviour = new TrainingDummy(unitObj);
        unitObj.SetBehaviour(behaviour, 0, UnitGroupType.HOSTILE, 1);
        manager.allUnits.Add(behaviour);
    }

    int GetCountOfEnemy()
    {
        int result = 0;
        foreach (var item in manager.allUnits)
        {
            if (item.group == UnitGroupType.HOSTILE)
            {
                result++;
            }
        }
        return result;
    }

}
