using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameMode : IGameModeBehaviour
{
    InGameManager manager;

    public TestGameMode(InGameManager _manager)
    {
        manager = _manager;
        
    }

    public IEnumerator GameModeRoutine()
    {
        while (true)
        {
            SetUnitsState(BehaviourState.RETREAT, UnitGroupType.ALLY);
            SpawnEnemy();
            yield return manager.StartCoroutine(ComingFront(5f));
            SetUnitsState(BehaviourState.INCOMBAT, UnitGroupType.ALLY);
            SetUnitsState(BehaviourState.INCOMBAT, UnitGroupType.HOSTILE);
            yield return new WaitUntil(() => GetCountOfEnemy() <= 0);
            SetUnitsState(BehaviourState.STANDBY, UnitGroupType.ALLY);
            yield return new WaitForSeconds(3f);
        }

        
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
        int[] spawnIdx = new int[4] { 47, 50, 53, 56 };
        for (int i = 0; i < 4; i++)
        {
            var unitObj = Object.Instantiate(manager.unitObjPrefab, manager.posList[spawnIdx[i] + 20], Quaternion.identity);
            manager.movingObjects.Add(unitObj.gameObject);

            var behaviour = manager.SetBehaviourInObject(unitObj, 0, UnitGroupType.HOSTILE);
            behaviour.range = 4;

            manager.allUnits.Add(behaviour);
        }
    }

    IEnumerator ComingFront(float dur)
    {
        foreach (var obj in manager.movingObjects)
        {
            manager.StartCoroutine(MoveSingleObj(obj, dur, 10f));
        }
        yield return new WaitForSeconds(dur);
        yield break;
    }

    IEnumerator MoveSingleObj(GameObject obj, float dur, float move)
    {
        float timer = 0f;
        Vector3 start = obj.transform.position;
        Vector3 end = obj.transform.position + new Vector3(-move, 0, 0);
        while (timer < dur)
        {
            obj.transform.position = Vector3.Lerp(start, end, timer / dur);
            timer += Time.deltaTime;
            yield return null;
        }
        obj.transform.position = end;
    }
}
