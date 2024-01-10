using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyGamePlayer : GamePlayer
{
    public TestEnemyGamePlayer(DeckData[] _unitIdx, Vector3[][] _posIdx, UnitGroupType _group) : base(_unitIdx, _posIdx, _group)
    {

    }

    public Coroutine MoveUnitsFront(bool isMoveAni)
    {
        if (isMoveAni)
        {
            AllUnitsMoveFront();
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
