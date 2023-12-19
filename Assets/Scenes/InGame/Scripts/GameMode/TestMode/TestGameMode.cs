using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameMode : IGameModeBehaviour
{
    int[] spawnIdx = new int[5] { 33, 30, 27, 24, 21 };

    InGameManager manager;
    
    GamePlayer player;
    GamePlayer enemyPlayer;

    public TestGameMode(InGameManager _manager)
    {
        manager = _manager;

        player = new PlayableGamePlayer(TempData.Instance.curDeckUnits, spawnIdx, UnitGroupType.ALLY, InGameManager.Instance.skillCanvas);
        int[] enemySpawnIdx = new int[4] { 47, 50, 53, 56 };
        enemyPlayer = new TestEnemyGamePlayer(new int[] { 0, 0, 0, 0 }, enemySpawnIdx, UnitGroupType.HOSTILE);
    }

    public IEnumerator GameModeRoutine()
    {
        player.SpawnCharacter();

        while (true)
        {
            SetUnitsState(BehaviourState.RETREAT, UnitGroupType.ALLY);
            enemyPlayer.SpawnCharacter();

            yield return manager.StartCoroutine(ComingFront(5f));
            SetUnitsState(BehaviourState.INCOMBAT, UnitGroupType.ALLY);
            SetUnitsState(BehaviourState.INCOMBAT, UnitGroupType.HOSTILE);

            while (true)
            {
                if(GetCountOf(UnitGroupType.HOSTILE) <= 0)
                {
                    break;
                }

                if(GetCountOf(UnitGroupType.ALLY) <= 0)
                {
                    yield break;
                }
                yield return null;
            }

            yield return new WaitUntil(() => WaitUntilEveryActionEnd());
            SetUnitsState(BehaviourState.STANDBY, UnitGroupType.ALLY);
        }
    }

    int GetCountOf(UnitGroupType group)
    {
        int result = 0;
        foreach (var item in manager.allUnits)
        {
            if (item.group == group)
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

    public void Update()
    {
        player?.Update();
        enemyPlayer?.Update();
    }
}
