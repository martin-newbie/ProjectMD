using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InGameManager : MonoBehaviour
{

    public static InGameManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public UnitObject unitObjPrefab;
    public SkeletonDataAsset[] humanoidDataAsset;

    [HideInInspector] public List<UnitObject> allUnits = new List<UnitObject>();
    List<Vector3> posList = new List<Vector3>();

    void Start()
    {
        InitTileList();
        TestMethod_InitUnits();
    }

    void InitTileList()
    {
        float mapHalfSize = 10f; // 10 is test, it should loaded from map data

        for (float i = -mapHalfSize; i <= mapHalfSize; i += 0.5f)
        {
            posList.Add(new Vector3(i, 0));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TestMethod_CombatStart();
        }
    }

    void TestMethod_InitUnits()
    {
        for (int i = 0; i < 4; i++)
        {
            var unitObj = Instantiate(unitObjPrefab, posList[i], Quaternion.identity);

            var behaviour = SetBehaviourInObject(unitObj, i, UnitGroupType.ALLY);
            behaviour.range = (4 - i) * 2 + 2;

            allUnits.Add(unitObj);
        }

        for (int i = 0; i < 4; i++)
        {
            var unitObj = Instantiate(unitObjPrefab, posList[posList.Count - i - 1], Quaternion.identity);

            var behaviour = SetBehaviourInObject(unitObj, 0, UnitGroupType.HOSTILE);
            behaviour.range = 4;

            allUnits.Add(unitObj);
        }
    }

    void TestMethod_CombatStart()
    {
        foreach (var item in allUnits)
        {
            item.behaviour.state = BehaviourState.INCOMBAT;
        }
    }

    UnitBehaviour SetBehaviourInObject(UnitObject unitObj, int idx, UnitGroupType group)
    {
        UnitBehaviour behaviour = null;

        switch (idx)
        {
            case 0:
                behaviour = new Seongah(unitObj);
                break;
            case 1:
                behaviour = new Minel(unitObj);
                break;
            case 2:
                behaviour = new Nina(unitObj);
                break;
            case 3:
                behaviour = new Asis(unitObj);
                break;
        }

        unitObj.InjectBehaviour(behaviour, humanoidDataAsset[idx], group);
        return behaviour;
    }

    public UnitBehaviour FindNearestTarget(UnitGroupType group, Vector3 startPos)
    {
        var groupUnits = allUnits.FindAll((item) => item.behaviour.group == group);

        float dist = float.MaxValue;
        UnitBehaviour result = null;

        foreach (var item in groupUnits)
        {
            float calc = Vector3.Distance(item.transform.position, startPos);
            if (calc < dist)
            {
                dist = calc;
                result = item.behaviour;
            }
        }

        return result;
    }

    public Vector3 GetPreferPos(UnitBehaviour subject, UnitBehaviour target, float range)
    {
        List<(Vector3, int)> resultList = new List<(Vector3, int)>(); 

        foreach (var item in posList)
        {
            if (Vector3.Distance(item, target.transform.position) <= range)
            {
                resultList.Add((item, GetPosCount(item, subject)));
            }
        }

        return resultList.OrderBy((item) => Vector3.Distance(subject.transform.position, item.Item1) + item.Item2 * 100).ElementAt(0).Item1;
    }

    public Vector3 GetNextPos(UnitBehaviour subject, UnitBehaviour target, float range)
    {
        Vector3 final = GetPreferPos(subject, target, range);
        Vector3 startPos = subject.transform.position;

        if (final.x > startPos.x)
        {
            startPos.x += 0.5f;
            return startPos;
        }
        if (final.x < startPos.x)
        {
            startPos.x -= 0.5f;
            return startPos;
        }

        return startPos;
    }

    int GetPosCount(Vector3 locate, UnitBehaviour subject)
    {
        var length = allUnits.FindAll((item) => item.behaviour.targetPos == locate && item.behaviour != subject).Count;
        return length;
    }
}
