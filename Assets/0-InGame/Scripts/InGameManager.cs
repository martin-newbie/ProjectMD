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

    void Start()
    {
        TestMethod_InitUnits();
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
            var unitObj = Instantiate(unitObjPrefab, new Vector3(-8f + i * 1.5f, 0), Quaternion.identity);

            var behaviour = SetBehaviourInObject(unitObj, i, UnitGroupType.ALLY);
            behaviour.range = (4 - i) * 2 + 2;

            allUnits.Add(unitObj);
        }

        for (int i = 0; i < 4; i++)
        {
            var unitObj = Instantiate(unitObjPrefab, new Vector3(2f + i * 1.5f, 0), Quaternion.identity);

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

    public UnitBehaviour FindNearestTarget(UnitGroupType group, float startPosX)
    {
        var groupUnits = allUnits.FindAll((item) => item.behaviour.group == group);

        float dist = float.MaxValue;
        UnitBehaviour result = null;

        foreach (var item in groupUnits)
        {
            float calc = Mathf.Abs(item.transform.position.x - startPosX);
            if(calc < dist)
            {
                dist = calc;
                result = item.behaviour;
            }
        }
        
        return result;
    }

}
