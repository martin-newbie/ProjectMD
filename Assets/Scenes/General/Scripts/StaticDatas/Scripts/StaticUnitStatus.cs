using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "StaticUnitStatus", menuName = "MD/ScriptableData/StaticUnitStatus", order = int.MinValue)]
public class StaticUnitStatus : SheetDataBase
{
    protected override string gid => "0";

    protected override string range => "E4:W29";

    public List<UnitStatus> dataList;

    protected override void SetData(string data)
    {
        dataList = new List<UnitStatus>();

        var datas = data.Split('\n');
        foreach (var item in datas)
        {
            dataList.Add(new UnitStatus(item.Split('\t')));
        }
    }
}

[System.Serializable]
public class UnitStatus
{
    public int keyIndex;
    [SerializeField] public Dictionary<StatusType, float> defaultValueList;
    [SerializeField] public Dictionary<StatusType, float> growthValueList;

    public UnitStatus(string[] data)
    {
        defaultValueList = new Dictionary<StatusType, float>();
        growthValueList = new Dictionary<StatusType, float>();

        int idx = 0;
        keyIndex = int.Parse(data[idx++]);
        defaultValueList.Add(StatusType.HP, float.Parse(data[idx++]));
        growthValueList.Add(StatusType.HP, float.Parse(data[idx++]));
        defaultValueList.Add(StatusType.DMG, float.Parse(data[idx++]));
        growthValueList.Add(StatusType.DMG, float.Parse(data[idx++]));
        defaultValueList.Add(StatusType.DEF, float.Parse(data[idx++]));
        growthValueList.Add(StatusType.DEF, float.Parse(data[idx++]));
        defaultValueList.Add(StatusType.DODGE, float.Parse(data[idx++]));
        defaultValueList.Add(StatusType.ACCURACY, float.Parse(data[idx++]));
        defaultValueList.Add(StatusType.CRI_RATE, float.Parse(data[idx++]));
        defaultValueList.Add(StatusType.CRI_DAMAGE, float.Parse(data[idx++]));
        defaultValueList.Add(StatusType.STABLE, float.Parse(data[idx++]));
        defaultValueList.Add(StatusType.RPM, float.Parse(data[idx++]));
        defaultValueList.Add(StatusType.ATK_DELAY, float.Parse(data[idx++]));
        defaultValueList.Add(StatusType.MOVE_SPEED, float.Parse(data[idx++]));
        defaultValueList.Add(StatusType.RANGE, float.Parse(data[idx++]));
        defaultValueList.Add(StatusType.CC_RESIST, float.Parse(data[idx++]));
        defaultValueList.Add(StatusType.COST_RECOVERY, float.Parse(data[idx++]));
        defaultValueList.Add(StatusType.SKILL_RECOVERY, float.Parse(data[idx++]));
        defaultValueList.Add(StatusType.ATK_TIMESCALE, 1f);
    }

    public float GetTotalStatus(StatusType type, int level = 0)
    {
        float result = 0f;
        float defaultValue = 0f;
        float increaseValue = 1f;

        if (defaultValueList.ContainsKey(type))
        {
            defaultValue = defaultValueList[type];
            result = defaultValue;
        }
        if (growthValueList.ContainsKey(type))
        {
            increaseValue = growthValueList[type];
            result = GetLevelStatus(level, defaultValue, increaseValue);
        }

        return result;
    }

    public Dictionary<StatusType, float> GetCalculatedValueDictionary(int level = 0)
    {
        var result = new Dictionary<StatusType, float>();
        foreach (var key in defaultValueList.Keys)
        {
            result.Add(key, GetTotalStatus(key, level));
        }
        return result;
    }

    float GetLevelStatus(int level, float defaultValue, float growthValue)
    {
        return defaultValue + growthValue * (1 - Mathf.Pow((float)System.Math.E, -0.03f * level));
    }
}

public enum StatusType
{
    HP,
    DMG,
    DEF,
    DODGE,      // 회피
    ACCURACY,   // 명중
    CRI_RATE,   // 크리 확률
    CRI_DAMAGE, // 크리 데미지
    STABLE,     // 안정치
    RPM,        
    ATK_DELAY,  // 공격과 공격 사이 딜레이
    MOVE_SPEED,
    RANGE,
    CC_RESIST,
    COST_RECOVERY,
    SKILL_RECOVERY,
    PENETRATE,
    ATK_TIMESCALE, // 공격 관련 사용되는 모든 애니메이션의 타임스케일, 기본값은 1
}