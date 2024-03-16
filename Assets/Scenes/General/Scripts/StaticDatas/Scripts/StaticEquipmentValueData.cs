using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "StaticEquipmentData", menuName = "MD/ScriptableData/StaticEquipmentData", order = int.MinValue)]
public class StaticEquipmentValueData : SheetDataBase
{
    protected override string gid => "634371692";

    protected override string range => "D3:I26";

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

[System.Serializable]
public class EquipmentValueData
{
    public int slot;
    public int tier;
    public int max_level;
    public int[] buff_type;
    public int[] max_value;
    public int[] min_value;

    public EquipmentValueData(string[] data)
    {
        int idx = 0;
        slot = int.Parse(data[idx++]);
        tier = int.Parse(data[idx++]);
        max_level = int.Parse(data[idx++]);
        buff_type = data[idx++].GetSplitCommaInt();
        max_value = data[idx++].GetSplitCommaInt();
        min_value = data[idx++].GetSplitCommaInt();
    }
}