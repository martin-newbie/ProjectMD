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
    }

    public IEnumerator GameModeRoutine()
    {
        player.ShowUnits(0);

        while (true)
        {
            enemyPlayer.ShowUnits(0);

            player.SetUnitsState(BehaviourState.INCOMBAT);
            enemyPlayer.SetUnitsState(BehaviourState.INCOMBAT);

            while (true)
            {
                if(enemyPlayer.GetCountOfUnits() <= 0)
                {
                    enemyPlayer.OnEnd();
                    break;
                }

                if(player.GetCountOfUnits() <= 0)
                {
                    player.OnEnd();
                    goto StageEnd;
                }
                yield return null;
            }

            yield return new WaitUntil(() => WaitUntilEveryActionEnd());
            player.ReturnOriginPos();
            yield return new WaitUntil(() => WaitUntilEveryActionEnd());

            foreach (var item in player.curUnits)
            {
                item.SetModelRotByDir(1);
            }

            player.SetUnitsState(BehaviourState.STANDBY);
            yield return new WaitForSeconds(1f);
        }

    StageEnd:
        yield break;
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

    public void Update()
    {
        player?.Update();
        enemyPlayer?.Update();
    }
}
