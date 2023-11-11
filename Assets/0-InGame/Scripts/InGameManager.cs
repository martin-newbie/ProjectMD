using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
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
        if(Input.GetKeyDown(KeyCode.Space))
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
            behaviour.range = i * 2 + 2;

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
            if(calc < dist)
            {
                dist = calc;
                result = item.behaviour;
            }
        }
        
        return result;
    }

    public Vector3 GetPreferPos(Vector3 start, Vector3 target, float range)
    {
        Vector3 result = new Vector3();
        float dist = float.MaxValue;
        foreach (var item in posList)
        {
            float calc = Vector3.Distance(item, start);
            if(calc < dist && Vector3.Distance(item, target) <= range)
            {
                dist = calc;
                result = item;
            }
        }
        return result;
    }

    public Vector3 GetNextPos(Vector3 start, Vector3 target, float range)
    {
        Vector3 final = GetPreferPos(start, target, range);
        
        if(final.x > start.x)
        {
            start.x += 0.5f;
            return start;
        }
        if(final.x < start.x)
        {
            start.x -= 0.5f;
            return start;
        }

        return start;
    }
}
