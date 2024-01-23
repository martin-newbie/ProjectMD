using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyGamePlayer : GamePlayer
{
    int stageLevel;

    public TestEnemyGamePlayer(DeckData[] _unitIdx, Vector3[][] _posIdx, UnitGroupType _group, int _stageLevel) : base(_unitIdx, _posIdx, _group)
    {
        stageLevel = _stageLevel;
    }

    public Coroutine MoveUnitsFront(bool isMoveAni)
    {
        if (isMoveAni)
        {
            AllUnitsPlayAnim();
        }

        return InGameManager.Instance.StartCoroutine(UnitsMove());
    }

    protected override UnitBehaviour SpawnUnit(int unitIdx, Vector3 pos)
    {
        return InGameManager.Instance.SpawnUnit(pos, unitIdx, group, stageLevel, 0);
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
