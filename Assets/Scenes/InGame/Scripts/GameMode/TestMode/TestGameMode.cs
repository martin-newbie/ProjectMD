using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameMode : IGameModeBehaviour
{
    int[] spawnIdx = new int[5] { 33, 30, 27, 24, 21 };
    bool isCombat;

    InGameManager manager;

    GamePlayer player;
    GamePlayer enemyPlayer;

    public TestGameMode(InGameManager _manager)
    {
        manager = _manager;

        int[][] posIdx = new int[4][];
        for (int i = 0; i < 4; i++)
        {
            posIdx[i] = spawnIdx;
        }

        int[][] enemyIdx = new int[4][];
        for (int i = 0; i < 4; i++)
        {
            enemyIdx[i] = new int[5] { 0, 0, 0, 0, 0 };
        }

        player = new PlayableGamePlayer(UserData.Instance.allDeckUnits.ToArray(), posIdx, UnitGroupType.ALLY, manager.skillCanvas);
        enemyPlayer = new TestEnemyGamePlayer(enemyIdx, posIdx, UnitGroupType.HOSTILE);

        player.ShowUnits(0);
    }

    public IEnumerator GameModeRoutine()
    {
        int wave = 0;
        while (wave < 4)
        {
            yield return new WaitUntil(() => isCombat);
            enemyPlayer.ShowUnits(wave);

            yield return new WaitUntil(() => enemyPlayer.curUnits.Count <= 0);
            isCombat = false;
            wave++;
        }
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
