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
    [HideInInspector] public List<Vector3> posList = new List<Vector3>();

    [HideInInspector] public List<GameObject> movingObjects = new List<GameObject>();

    int gameModeIdx = 0;
    IGameModeBehaviour gameMode;

    void Start()
    {
        InitTileList();
        InitSkills();
        InitGameMode();
        StartCoroutine(gameMode.GameModeRoutine());
    }

    public void InitSkill()
    {
        var playable = allUnits.FindAll((item) => item.group == UnitGroupType.ALLY).Select((item)=> item as ActiveSkillBehaviour).ToArray();
        SkillManager.Instance.InitSkills(playable);
        SkillManager.Instance.StartGame();
    }
    
    void InitTileList()
    {
        float mapHalfSize = 20f;
        for (float i = -mapHalfSize; i <= mapHalfSize; i += 0.5f) { posList.Add(new Vector3(i, 0)); }
    }

    void InitGameMode()
    {
        switch (gameModeIdx)
        {
            case 0:
                gameMode = new TestBossGameMode(this);
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TestMethod_CombatStart();
        }
    }

    void InitSkills()
    {
        var skillAble = allUnits.FindAll((item) => item.group == UnitGroupType.ALLY).Select((item) => item as ActiveSkillBehaviour);
        SkillManager.Instance.InitSkills(skillAble.ToArray());
    }

    void TestMethod_CombatStart()
    {
        foreach (var item in allUnits)
        {
            item.state = BehaviourState.INCOMBAT;
        }
        SkillManager.Instance.StartGame();
    }

    public UnitBehaviour SpawnUnit(Vector3 spawnPos, int idx, UnitGroupType group, int barType)
    {
        var unitObj = SpawnUnitObject(spawnPos);
        var behaviour = SetBehaviourInObject(unitObj, idx, group, barType);
        return behaviour;
    }

    public UnitObject SpawnUnitObject(Vector3 spawnPos)
    {
        return Instantiate(unitObjPrefab, spawnPos, Quaternion.identity);
    }

    public UnitBehaviour SetBehaviourInObject(UnitObject unitObj, int idx, UnitGroupType group, int barType)
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

        unitObj.SetBehaviour(behaviour, idx, group, barType);
        return behaviour;
    }

    public void RetireCharacter(UnitBehaviour unit)
    {
        if (unit is ActiveSkillBehaviour)
        {
            SkillManager.Instance.RemoveCharacterAtSkills(unit as ActiveSkillBehaviour);
        }

        allUnits.Remove(unit);
        movingObjects.Add(unit.gameObject);
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
        var length = allUnits.FindAll((item) => item.targetPos == locate && item != subject).Count;
        return length;
    }
}
