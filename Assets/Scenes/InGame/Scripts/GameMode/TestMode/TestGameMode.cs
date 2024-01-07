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

        Vector3[][] userPoses = new Vector3[4][];
        for (int i = 0; i < 4; i++)
        {
            userPoses[i] = new Vector3[5];
            for (int j = 0; j < 5; j++)
            {
                userPoses[i][j] = new Vector3(-2 - (1.5f * j), 0, 0);
            }
        }

        Vector3[][] enemyPos = new Vector3[4][];
        int[][] enemyIdx = new int[4][];
        for (int i = 0; i < 4; i++)
        {
            enemyIdx[i] = new int[4] { 0, 0, 0, 0 };
            enemyPos[i] = new Vector3[4];
            for (int j = 0; j < 4; j++)
            {
                enemyPos[i][j] = new Vector3(2 + (1.5f * j), 0, 0);
            }
        }

        player = new PlayableGamePlayer(UserData.Instance.allDeckUnits.ToArray(), userPoses, UnitGroupType.ALLY, manager.skillCanvas);
        enemyPlayer = new TestEnemyGamePlayer(enemyIdx, enemyPos, UnitGroupType.HOSTILE);
        player.ShowUnits(0);
    }

    public IEnumerator GameModeRoutine()
    {
        int wave = 0;
        yield return new WaitUntil(() => isCombat);
        player.StartGame();
        enemyPlayer.StartGame();

        while (wave < 4)
        {
            enemyPlayer.ShowUnits(wave);

            player.SetUnitsState(BehaviourState.INCOMBAT);
            enemyPlayer.SetUnitsState(BehaviourState.INCOMBAT);

            yield return new WaitUntil(() => enemyPlayer.curUnits.Count <= 0);

            yield return new WaitUntil(() => WaitUntilEveryActionEnd(player.curUnits.ToArray()));
            player.ReturnOriginPos();
            yield return new WaitUntil(() => WaitUntilEveryActionEnd(player.curUnits.ToArray()));

            player.SetUnitsState(BehaviourState.STANDBY);
            isCombat = false;
            wave++;
        }
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

    public void Update()
    {
        player?.Update();
        enemyPlayer?.Update();

        if (!isCombat)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                player.ShowUnits(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                player.ShowUnits(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                player.ShowUnits(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                player.ShowUnits(3);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                isCombat = true;
            }
        }
    }
}
