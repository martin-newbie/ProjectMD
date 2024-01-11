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
    public SkillCanvas skillCanvas;
    public BackgroundScroll scrollMap;

    [HideInInspector] public List<UnitBehaviour> allUnits = new List<UnitBehaviour>();
    [HideInInspector] public List<GameObject> movingObjects = new List<GameObject>();

    int gameModeIdx = 0;
    IGameModeBehaviour gameMode;

    void Start()
    {
        InitGameMode();
        StartCoroutine(gameMode.GameModeRoutine());
    }

    private void Update()
    {
        gameMode?.Update();
    }

    public void InitSkill()
    {
        var playable = allUnits.FindAll((item) => item.group == UnitGroupType.ALLY).Select((item) => item as ActiveSkillBehaviour).ToArray();
    }

    public Coroutine MapScrollFor(float dur, float speed)
    {
        StartCoroutine(scrollMap.ScrollBackgroundFor(dur, speed));
        return null;
    }

    void InitGameMode()
    {
        switch (gameModeIdx)
        {
            case 0:
                gameMode = new TestGameMode(this);
                break;
        }
    }

    public UnitBehaviour SpawnUnit(Vector3 spawnPos, int idx, UnitGroupType group, int level, int barType)
    {
        var unitObj = SpawnUnitObject(spawnPos);
        var behaviour = SetBehaviourInObject(unitObj, idx, group, level, barType);
        return behaviour;
    }

    public UnitObject SpawnUnitObject(Vector3 spawnPos)
    {
        return Instantiate(unitObjPrefab, spawnPos, Quaternion.identity);
    }

    public UnitBehaviour SetBehaviourInObject(UnitObject unitObj, int idx, UnitGroupType group, int level, int barType)
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
                behaviour = new Asis(unitObj);
                break;
            case 3:
                behaviour = new Ilena(unitObj);
                break;
            case 4:
                behaviour = new Luria(unitObj);
                break;
            case 17:
                behaviour = new Fei(unitObj);
                break;
            case 18:
                behaviour = new Lada(unitObj);
                break;
            case 22:
                behaviour = new Yui(unitObj);
                break;
            case 24:
                behaviour = new Nina(unitObj);
                break;
            case 25:
                behaviour = new Aiden(unitObj);
                break;
        }

        unitObj.SetBehaviour(behaviour, idx, group, level, barType);
        return behaviour;
    }

    public UnitBehaviour FindNearestTarget(UnitGroupType group, Vector3 startPos)
    {
        var groupUnits = allUnits.FindAll((item) => item.group == group);

        float dist = float.MaxValue;
        UnitBehaviour result = null;

        foreach (var item in groupUnits)
        {
            float calc = Vector3.Distance(item.transform.position, startPos);
            if (calc < dist)
            {
                dist = calc;
                result = item;
            }
        }

        return result;
    }
}
