using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "StaticConstUnitData", menuName = "MD/ScriptableData/StaticConstUnitData", order = int.MinValue)]
public class StaticConstUnitData : SheetDataBase
{
    protected override string gid => "930077113";

    protected override string range => "C4:N33";

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
    public int modelIndex;
    public string name;
    public int defaultClass;
    public int ammoCount;
    public int burstCount;
    public int[] raceType;
    public int position;
    public float[] damageToRace;

    // 추후 대체 및 삭제 예정
    public int atkType;
    public int defType;

    public ConstUnitData(string[] data)
    {
        int idx = 0;
        damageToRace = new float[4];

        keyIndex = int.Parse(data[idx++]);
        modelIndex = int.Parse(data[idx++]);
        name = data[idx++];
        defaultClass = int.Parse(data[idx++]);
        ammoCount = int.Parse(data[idx++]);
        burstCount = int.Parse(data[idx++]);
        raceType = data[idx++].Split(',').ToList().Select(item => int.Parse(item)).ToArray();
        position = int.Parse(data[idx++]);
        damageToRace[0] = float.Parse(data[idx++]);
        damageToRace[1] = float.Parse(data[idx++]);
        damageToRace[2] = float.Parse(data[idx++]);
        damageToRace[3] = float.Parse(data[idx++]);
    }
}
