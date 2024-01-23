using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyGamePlayer : GamePlayer
{
    int stageLevel;

    public TestEnemyGamePlayer(UnitData[] unitDatas, UnitGroupType _group, int _stageLevel) : base(_group)
    {
        stageLevel = _stageLevel;
    }

    public Coroutine MoveUnitsFront(bool isMoveAni)
    {
        if (isMoveAni)
        {
            AllUnitsPlayAnim("battle_move", true);
        }

        return InGameManager.Instance.StartCoroutine(UnitsMove());
    }

    IEnumerator UnitsMove()
    {
        float timer = 0f;
        float speed = 2.5f;

        while (timer < 10)
        {
            foreach (var item in curUnits)
            {
                item.transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
            timer += speed * Time.deltaTime;
            yield return null;
        }

        yield break;
    }
}
