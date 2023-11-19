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
    public Sprite[] characterProfiles;

    [HideInInspector] public List<UnitBehaviour> allUnits = new List<UnitBehaviour>();
    List<Vector3> posList = new List<Vector3>();

    [HideInInspector] public List<GameObject> movingObjects = new List<GameObject>();

    void Start()
    {
        InitTileList();
        InitSkills();
        StartCoroutine(TestGameLogic());
    }

    void InitTileList()
    {
        float mapHalfSize = 20f;
        for (float i = -mapHalfSize; i <= mapHalfSize; i += 0.5f) { posList.Add(new Vector3(i, 0)); }
    }

    IEnumerator TestGameLogic()
    {
        int[] spawnIdx = new int[4] { 2, 6, 10, 14 };
        for (int i = 0; i < 4; i++)
        {
            var unitObj = Instantiate(unitObjPrefab, posList[spawnIdx[i]], Quaternion.identity);

            var behaviour = SetBehaviourInObject(unitObj, i, UnitGroupType.ALLY);
            behaviour.range = (4 - i) * 2 + 2;

            allUnits.Add(behaviour);
        }

        while (true)
        {
            spawnEnemy();
            yield return StartCoroutine(comingFront(5f));
            yield return new WaitUntil(() => getCountOfEnemy() <= 0);
        }

        int getCountOfEnemy()
        {
            int result = 0;
            foreach (var item in allUnits)
            {
                if (item.group == UnitGroupType.HOSTILE)
                {
                    result++;
                }
            }
            return result;
        }
        void spawnEnemy()
        {
            for (int i = 0; i < 4; i++)
            {
                var unitObj = Instantiate(unitObjPrefab, posList[0], Quaternion.identity);

                var behaviour = SetBehaviourInObject(unitObj, 0, UnitGroupType.HOSTILE);
                behaviour.range = 4;

                allUnits.Add(behaviour);
            }
        }
        IEnumerator comingFront(float dur)
        {
            foreach (var obj in movingObjects)
            {
                StartCoroutine(moveSingleObj(obj, dur, 5f));
            }
            yield return new WaitForSeconds(dur);
            yield break;
        }
        IEnumerator moveSingleObj(GameObject obj, float dur, float move)
        {
            float timer = 0f;
            Vector3 start = obj.transform.position;
            Vector3 end = obj.transform.position + new Vector3(move, 0, 0);
            while (timer < dur)
            {
                obj.transform.position = Vector3.Lerp(start, end, timer / dur);
                timer += Time.deltaTime;
                yield return null;
            }
            obj.transform.position = end;
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

        unitObj.InjectBehaviour(behaviour, idx, group);
        return behaviour;
    }

    public void RetireCharacter(UnitBehaviour unit)
    {
        if (unit is ActiveSkillBehaviour)
        {
            SkillManager.Instance.RemoveCharacterAtSkills(unit as ActiveSkillBehaviour);
        }

        allUnits.Remove(unit);
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
