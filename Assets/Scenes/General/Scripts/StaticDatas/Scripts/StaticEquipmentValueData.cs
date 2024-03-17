using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "StaticEquipmentData", menuName = "MD/ScriptableData/StaticEquipmentData", order = int.MinValue)]
public class StaticEquipmentValueData : SheetDataBase
{
    protected override string gid => "634371692";

    protected override string range => "D3:L26";

    public List<EquipmentValueData> dataList;

    protected override void SetData(string data)
    {
        dataList = new List<EquipmentValueData>();

        var datas = data.Split('\n');
        foreach (var item in datas)
        {
            dataList.Add(new EquipmentValueData(item.Split('\t')));
        }
    }
}

[Serializable]
public class EquipmentValueData
{
    public int index;
    public int slot;
    public int tier;
    public int min_level;
    public int max_level;
    public int[] buff_type;
    public int[] value_type;
    public float[] max_value;
    public float[] min_value;

    public EquipmentValueData(string[] data)
    {
        int idx = 0;
        index = int.Parse(data[idx++]);
        slot = int.Parse(data[idx++]);
        tier = int.Parse(data[idx++]);
        min_level = int.Parse(data[idx++]);
        max_level = int.Parse(data[idx++]);
        buff_type = data[idx++].GetSplitCommaInt();
        value_type = data[idx++].GetSplitCommaInt();
        min_value = data[idx++].GetSplitCommaFloat();
        max_value = data[idx++].GetSplitCommaFloat();
    }

    public float GetLevelBuff(int level, int index)
    {
        float minValue = min_value[Array.FindIndex(buff_type, item => item == buff_type[index])];
        float maxValue = max_value[Array.FindIndex(buff_type, item => item == buff_type[index])];
        float amount = (float)(level - min_level) / (float)(max_level - min_level);
        float value = (amount * (maxValue - minValue)) + minValue;
        return value;
    }
}