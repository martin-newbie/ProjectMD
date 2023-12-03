using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaticUnitStatus", menuName = "MD/ScriptableData/StaticUnitStatus", order = int.MinValue)]
public class StaticUnitStatus : SheetDataBase
{
    protected override string gid => "0";

    protected override string range => "C3:S13";

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
    [SerializeField] Dictionary<StatusType, float> statusDic;

    public UnitStatus(string[] data)
    {
        statusDic = new Dictionary<StatusType, float>();

        int idx = 0;
        keyIndex = int.Parse(data[idx++]);
        for (; idx < data.Length; idx++)
        {
            statusDic.Add((StatusType)(idx - 1), float.Parse(data[idx]));
        }
    }
}

public enum StatusType
{
    HP,
    HP_INCREASE,
    ATK,
    ATK_INCREASE,
    DEF,
    DEF_INCREASE,
    DEF_COUNTER,
    DODGE,
    ACCURACY,
    CRI,
    CRI_DAMAGE,
    STABLE,
    RPM,
    ATK_DELAY,
    MOVE_SPEED,
    RANGE,
}