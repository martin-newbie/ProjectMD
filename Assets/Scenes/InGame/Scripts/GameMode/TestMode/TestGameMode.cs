using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameMode : IGameModeBehaviour
{
    bool isCombat;

    InGameManager manager;

    GamePlayer player;
    GamePlayer enemyPlayer;

    public TestGameMode(InGameManager _manager)
    {
        manager = _manager;
    }

    public IEnumerator GameModeRoutine()
    {
        yield break;
    }

    public void Update()
    {

    }

    bool WaitUntilEveryActionEnd(UnitBehaviour[] units)
    {
        foreach (var item in units)
        {
            if (item.isAction)
            {
                return false;
            }
        }
        return true;
    }
}
