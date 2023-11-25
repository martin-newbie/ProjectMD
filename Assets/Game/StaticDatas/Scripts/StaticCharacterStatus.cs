using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaticCharacterStatus", menuName = "MD/ScriptableData/StaticCharacterStatus", order = int.MinValue)]
public class StaticCharacterStatus : SheetDataBase
{
    protected override string gid => "0";

    protected override string range => "C3:S13";

    public List<StaticStatusData> dataList;

    protected override void SetData(string data)
    {
        dataList = new List<StaticStatusData>();

        var datas = data.Split('\n');
        foreach (var item in datas)
        {
            dataList.Add(new StaticStatusData(item.Split('\t')));
        }
    }
}

[System.Serializable]
public class StaticStatusData
{
    public int keyIndex;
    [SerializeField] Dictionary<StatusType, float> statusDic;

    public StaticStatusData(string[] data)
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