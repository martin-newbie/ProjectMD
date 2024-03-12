using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaticSkillRequireItemData", menuName = "MD/ScriptableData/StaticSkillRequireItemData", order = int.MinValue)]
public class StaticSkillRequireItemData : SheetDataBase
{
    protected override string gid => "963626872";

    protected override string range => "C3:O28";

    public List<SkillRequireItemData> dataList;

    protected override void SetData(string data)
    {
        dataList = new List<SkillRequireItemData>();
        var splitData = data.Split('\n');
        foreach (var item in splitData)
        {
            dataList.Add(new SkillRequireItemData(item.Split('\t')));
        }
    }
}


public class SkillRequireItemData
{
    public int index;
    public int[] itemArray;

    public SkillRequireItemData(string[] data)
    {
        index = int.Parse(data[0]);
        itemArray = new int[12];
        for (int i = 0; i < 12; i++)
        {
            itemArray[i] = int.Parse(data[i + 1]);
        }
    }
}