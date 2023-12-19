using System.Collections;
using UnityEngine;

public class TestBossGameMode : IGameModeBehaviour
{
    int[] spawnIdx = { 33, 30, 27, 24, 21 };
    InGameManager manager;

    public TestBossGameMode(InGameManager _manager)
    {
        manager = _manager;
    }

    public IEnumerator GameModeRoutine()
    {
        var playablePlayer = new PlayableGamePlayer(TempData.Instance.curDeckUnits, spawnIdx, UnitGroupType.ALLY, manager.skillCanvas);
        int[] bossIdx = { 0 };
        int[] posIdx = { 50 };
        var bossPlayer = new TestBossGamePlayer(bossIdx, posIdx, UnitGroupType.HOSTILE);

        playablePlayer.SpawnCharacter();
        bossPlayer.SpawnCharacter();
        SetUnitsState(BehaviourState.INCOMBAT, UnitGroupType.ALLY);
        yield return new WaitUntil(() => GetCountOfEnemy() <= 0);
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
