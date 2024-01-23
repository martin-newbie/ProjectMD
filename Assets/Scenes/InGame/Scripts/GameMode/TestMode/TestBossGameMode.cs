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
    }

    public void Start()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        throw new System.NotImplementedException();
    }
}
