using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyGamePlayer : GamePlayer
{
    int stageLevel;
    List<List<UnitBehaviour>> waveList = new List<List<UnitBehaviour>>();

    public TestEnemyGamePlayer(UnitData[][] unitDatas, UnitGroupType _group, int _stageLevel) : base(_group)
    {
        waveList = new List<List<UnitBehaviour>>();
        stageLevel = _stageLevel;
        InitUnits(unitDatas, _group);
    }

    void InitUnits(UnitData[][] unitDatas, UnitGroupType group)
    {
        for (int i = 0; i < unitDatas.Length; i++)
        {
            waveList.Add(new List<UnitBehaviour>());

            var waveData = unitDatas[i];
            float range = 6 / waveData.Length;

            for (int j = 0; j < waveData.Length; j++)
            {
                var data = waveData[j];
                var spawnPos = new Vector3(i * 10 + 3 + range * j, 0, 0);
                var statusData = StaticDataManager.GetUnitStatus(data.index).GetCalculatedValueDictionary();
                var unit = InGameManager.Instance.SpawnUnit(spawnPos, group, data, statusData, 0);
                unit.InjectDeadEvent(() => RemoveActiveUnit(unit));
                waveList[i].Add(unit);
            }
        }
    }

    public void ActiveUnitAt(int idx)
    {
        var units = waveList[idx];
        foreach (var unit in units)
        {
            unit.SetBehaviourState(BehaviourState.INCOMBAT);
            AddActiveUnit(unit);
        }
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
