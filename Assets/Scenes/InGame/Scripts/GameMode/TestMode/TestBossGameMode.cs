using System.Collections;
using UnityEngine;

public class TestBossGameMode : IGameModeBehaviour
{
    int[] spawnIdx = { 33, 30, 27, 24, 21 };
    InGameManager manager;

    GamePlayer playablePlayer;
    GamePlayer bossPlayer;

    public TestBossGameMode(InGameManager _manager)
    {
        manager = _manager;

        int[] bossIdx = { 0 };
        int[] posIdx = { 50 };
        playablePlayer = new PlayableGamePlayer(TempData.Instance.curDeckUnits, spawnIdx, UnitGroupType.ALLY, manager.skillCanvas);
        bossPlayer = new TestBossGamePlayer(bossIdx, posIdx, UnitGroupType.HOSTILE);
    }

    public IEnumerator GameModeRoutine()
    {
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

    public void Update()
    {
        playablePlayer?.Update();
        bossPlayer?.Update();
    }
}
