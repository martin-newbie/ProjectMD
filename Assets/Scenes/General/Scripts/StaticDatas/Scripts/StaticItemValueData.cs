using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaticItemValueData", menuName = "MD/ScriptableData/StaticItemValueData", order = int.MinValue)]
public class StaticItemValueData : SheetDataBase
{
    protected override string gid => "1901022725";

    protected override string range => "C3:F163";

    public List<ItemValueData> dataList;

    protected override void SetData(string data)
    {
        dataList = new List<ItemValueData>();

        var datas = data.Split('\n');
        foreach (var item in datas)
        {
            dataList.Add(new ItemValueData(item.Split('\t')));
        }
    }
}

[System.Serializable]
public class ItemValueData
{
    public int index;
    public int value;
    public int species;
    public int rate;

    public ItemValueData(string[] data)
    {
        int idx = 0;
        index = int.Parse(data[idx++]);
        value = int.Parse(data[idx++]);
        species = int.Parse(data[idx++]);
        rate = int.Parse(data[idx++]);
    }
}