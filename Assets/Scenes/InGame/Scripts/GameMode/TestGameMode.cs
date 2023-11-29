using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameMode : IGameModeBehaviour
{
    int[] spawnIdx = new int[5] { 21, 24, 27, 30, 33 };

    InGameManager manager;

    public TestGameMode(InGameManager _manager)
    {
        manager = _manager;

    }

    public IEnumerator GameModeRoutine()
    {
        SpawnAllyUnits();

        while (true)
        {
            SetUnitsState(BehaviourState.RETREAT, UnitGroupType.ALLY);
            SpawnEnemy();
            yield return manager.StartCoroutine(ComingFront(5f));
            SetUnitsState(BehaviourState.INCOMBAT, UnitGroupType.ALLY);
            SetUnitsState(BehaviourState.INCOMBAT, UnitGroupType.HOSTILE);
            yield return new WaitUntil(() => GetCountOfEnemy() <= 0);
            yield return new WaitUntil(() => WaitUntilEveryActionEnd());
            SetUnitsState(BehaviourState.STANDBY, UnitGroupType.ALLY);
        }
    }

    void SpawnAllyUnits()
    {
        int[] units = TempData.Instance.curDeckUnits;
        for (int i = 0; i < units.Length; i++)
        {
            var unitObj = Object.Instantiate(manager.unitObjPrefab, manager.posList[spawnIdx[i]], Quaternion.identity);

            var behaviour = manager.SetBehaviourInObject(unitObj, units[i], UnitGroupType.ALLY);
            behaviour.range = (4 - i) * 2 + 2;

            manager.allUnits.Add(behaviour);
        }

        manager.InitSkill();
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

    bool WaitUntilEveryActionEnd()
    {
        int allyCnt = 0;
        int endCnt = 0;

        foreach (var item in manager.allUnits)
        {
            if (item.group == UnitGroupType.ALLY)
            {
                allyCnt++;
            }
        }

        foreach (var item in manager.allUnits)
        {
            if (item.group == UnitGroupType.ALLY && !item.isAction)
            {
                endCnt++;
            }
        }

        return allyCnt == endCnt;
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
