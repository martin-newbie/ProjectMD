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

    List<UnitObject> allUnits = new List<UnitObject>();

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

            var behaviour = new Seongah(unitObj);
            behaviour.group = UnitGroupType.ALLY;
            behaviour.range = (4 - i) * 2 + 2;
            behaviour.moveSpeed = 3;
            unitObj.InjectBehaviour(behaviour);

            allUnits.Add(unitObj);
        }

        for (int i = 0; i < 4; i++)
        {
            var unitObj = Instantiate(unitObjPrefab, new Vector3(2f + i * 1.5f, 0), Quaternion.identity);

            var behaviour = new Seongah(unitObj);
            behaviour.group = UnitGroupType.HOSTILE;
            behaviour.range = i * 2 + 2;
            behaviour.moveSpeed = 3;
            unitObj.InjectBehaviour(behaviour);

            allUnits.Add(unitObj);
        }

        unitObjPrefab.gameObject.SetActive(false);
    }

    void TestMethod_CombatStart()
    {
        foreach (var item in allUnits)
        {
            item.behaviour.state = BehaviourState.INCOMBAT;
        }
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
