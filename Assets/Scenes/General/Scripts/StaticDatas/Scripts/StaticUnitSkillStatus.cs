using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaticUnitSkillStatus", menuName = "MD/ScriptableData/StaticUnitSkillStatus", order = int.MinValue)]
public class StaticUnitSkillStatus : SheetDataBase
{
    protected override string gid => "1734931170";

    protected override string range => "D4:R29";

    public List<UnitSkillStatus> dataList;

    protected override void SetData(string data)
    {
        dataList = new List<UnitSkillStatus>();

        var datas = data.Split('\n');
        foreach (var item in datas)
        {
            dataList.Add(new UnitSkillStatus(item.Split('\t')));
        }
    }
}

[System.Serializable]
public class UnitSkillStatus
{
    public int unitIndex;
    public int cost;

    public float active_min;
    public float active_max;
    public float active_assist_1;
    public float active_assist_2;

    public float passive_min;
    public float passive_max;
    public float passive_assist;

    public float enforce_min;
    public float enforce_max;
    public float enforce_assist;

    public float sub_min;
    public float sub_max;
    public float sub_assist;

    public UnitSkillStatus(string[] data)
    {
        int idx = 0;

        unitIndex = int.Parse(data[idx++]);
        cost = int.Parse(data[idx++]);

        active_min = float.Parse(data[idx++]);
        active_max = float.Parse(data[idx++]);
        active_assist_1 = float.Parse(data[idx++]);
        active_assist_2 = float.Parse(data[idx++]);

        passive_min = float.Parse(data[idx++]);
        passive_max = float.Parse(data[idx++]);
        passive_assist = float.Parse(data[idx++]);

        enforce_min = float.Parse(data[idx++]);
        enforce_max = float.Parse(data[idx++]);
        enforce_assist = float.Parse(data[idx++]);

        sub_min = float.Parse(data[idx++]);
        sub_max = float.Parse(data[idx++]);
        sub_assist = float.Parse(data[idx++]);
    }

    public float GetActiveSkillValue(int skillLevel)
    {
        return ((active_max - active_min) / 5 * skillLevel + active_min) * 0.01f;
    }
}