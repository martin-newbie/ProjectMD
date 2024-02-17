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

    [HideInInspector] public List<UnitBehaviour> allUnits = new List<UnitBehaviour>();
    [HideInInspector] public List<GameObject> movingObjects = new List<GameObject>();

    int gameModeIdx = 0;
    IGameModeBehaviour gameMode;

    void Start()
    {
        InitGameMode();
        gameMode?.Start();
    }

    private void Update()
    {
        gameMode?.Update();
    }

    void InitGameMode()
    {
        switch (TempData.Instance.selectedGameMode)
        {
            case GameMode.NOTHING:
                break;
            case GameMode.TEST:
                gameMode = new TestGameMode(this);
                break;
            case GameMode.STAGE:
                gameMode = new StageGameMode(this);
                break;
            case GameMode.DUNGEON:
                break;
            case GameMode.RAID:
                break;
            case GameMode.PVP:
                break;
            default:
                break;
        }
    }

    public UnitBehaviour SpawnUnit(Vector3 spawnPos, UnitGroupType group, UnitData data, Dictionary<StatusType, float> status, int barType)
    {
        var unitObj = SpawnUnitObject(spawnPos);
        var behaviour = SetBehaviourInObject(unitObj, group, data, status, barType);
        return behaviour;
    }

    public UnitObject SpawnUnitObject(Vector3 spawnPos)
    {
        return Instantiate(unitObjPrefab, spawnPos, Quaternion.identity);
    }

    public UnitBehaviour SetBehaviourInObject(UnitObject unitObj, UnitGroupType group, UnitData data, Dictionary<StatusType, float> status, int barType)
    {
        UnitBehaviour behaviour = null;

        switch (data.index)
        {
            case 0:
                behaviour = new Seongah(data, status);
                break;
            case 1:
                behaviour = new Minel(data, status);
                break;
            case 2:
                behaviour = new Asis(data, status);
                break;
            case 3:
                behaviour = new Ilena(data, status);
                break;
            case 4:
                behaviour = new Luria(data, status);
                break;
            case 6:
                behaviour = new Picardy(data, status);
                break;
            case 17:
                behaviour = new Fei(data, status);
                break;
            case 18:
                behaviour = new Lada(data, status);
                break;
            case 22:
                behaviour = new Yui(data, status);
                break;
            case 24:
                behaviour = new Nina(data, status);
                break;
            case 25:
                behaviour = new Aiden(data, status);
                break;
            case 26:
                behaviour = new DieselRobotSMG(data, status);
                break;
        }

        unitObj.SetBehaviour(behaviour, group, barType);
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
