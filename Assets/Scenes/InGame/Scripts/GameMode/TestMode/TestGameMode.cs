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
        DeckData[] enemyData = new DeckData[4];
        for (int i = 0; i < 4; i++)
        {
            enemyData[i].unitsIdx = new int[4] { 0, 0, 0, 0 };
            enemyPos[i] = new Vector3[4];
            for (int j = 0; j < 4; j++)
            {
                enemyPos[i][j] = new Vector3(2 + (1.5f * j) + 10, 0, 0);
            }
        }

        player = new PlayableGamePlayer(UserData.Instance.decks, userPoses, UnitGroupType.ALLY, manager.skillCanvas);
        enemyPlayer = new TestEnemyGamePlayer(enemyData, enemyPos, UnitGroupType.HOSTILE);
        player.ShowUnits(0);
    }

    public IEnumerator GameModeRoutine()
    {
        int wave = 0;
        yield return new WaitUntil(() => isCombat);
        player.StartGame();
        enemyPlayer.StartGame();

        player.AllUnitsMoveFront();
        enemyPlayer.ShowUnits(wave);

        InGameManager.Instance.MapScrollFor(10f / 2.5f, 2.5f);
        yield return (enemyPlayer as TestEnemyGamePlayer).MoveUnitsFront(false);

        player.SetUnitsState(BehaviourState.INCOMBAT);
        enemyPlayer.SetUnitsState(BehaviourState.INCOMBAT);

        yield return new WaitUntil(() => enemyPlayer.curUnits.Count <= 0);
        yield return new WaitUntil(() => WaitUntilEveryActionEnd(player.curUnits.ToArray()));
        player.ReturnOriginPos();
        yield return new WaitUntil(() => WaitUntilEveryActionEnd(player.curUnits.ToArray()));
        player.SetUnitsState(BehaviourState.STANDBY);
        wave++;

        while (wave < 4)
        {
            enemyPlayer.ShowUnits(wave);
            yield return (enemyPlayer as TestEnemyGamePlayer).MoveUnitsFront(true);

            player.SetUnitsState(BehaviourState.INCOMBAT);
            enemyPlayer.SetUnitsState(BehaviourState.INCOMBAT);

            yield return new WaitUntil(() => enemyPlayer.curUnits.Count <= 0);
            yield return new WaitUntil(() => WaitUntilEveryActionEnd(player.curUnits.ToArray()));
            player.ReturnOriginPos();
            yield return new WaitUntil(() => WaitUntilEveryActionEnd(player.curUnits.ToArray()));
            player.SetUnitsState(BehaviourState.STANDBY);
            wave++;

        }

        yield break;
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
