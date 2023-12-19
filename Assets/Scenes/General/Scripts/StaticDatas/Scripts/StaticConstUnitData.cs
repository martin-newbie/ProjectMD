using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaticConstUnitData", menuName = "MD/ScriptableData/StaticConstUnitData", order = int.MinValue)]
public class StaticConstUnitData : SheetDataBase
{
    protected override string gid => "930077113";

    protected override string range => "C4:I29";

    public List<ConstUnitData> dataList;

    protected override void SetData(string data)
    {
        dataList = new List<ConstUnitData>();

        var datas = data.Split('\n');
        foreach (var item in datas)
        {
            dataList.Add(new ConstUnitData(item.Split('\t')));
        }
    }
}

[System.Serializable]
public class ConstUnitData
{
    public int keyIndex;
    public string name;
    public int defaultClass;
    public int ammoCount;
    public int burstCount;
    public int atkType;
    public int defType;

    public ConstUnitData(string[] data)
    {
        int idx = 0;

        keyIndex = int.Parse(data[idx++]);
        name = data[idx++];
        defaultClass = int.Parse(data[idx++]);
        ammoCount = int.Parse(data[idx++]);
        burstCount = int.Parse(data[idx++]);
        atkType = int.Parse(data[idx++]);
        defType = int.Parse(data[idx++]);
    }
}
