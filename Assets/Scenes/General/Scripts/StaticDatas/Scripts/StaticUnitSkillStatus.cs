using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "StaticUnitSkillStatus", menuName = "MD/ScriptableData/StaticUnitSkillStatus", order = int.MinValue)]
public class StaticUnitSkillStatus : SheetDataBase
{
    protected override string gid => "1734931170";

    protected override string range => "D4:T29";

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
    public int skill_type;
    public float active_min;
    public float active_max;
    public float active_time_min;
    public float active_time_max;
    public float passive_min;
    public float passive_max;
    public float passive_time_min;
    public float passive_time_max;
    public float used_min;
    public float used_max;
    public float used_sub;
    public float use_min;
    public float use_max;
    public float use_sub;

    public UnitSkillStatus(string[] data)
    {
        int idx = 0;

        unitIndex = int.Parse(data[idx++]);
        cost = int.Parse(data[idx++]);
        skill_type = int.Parse(data[idx++]);
        active_min = float.Parse(data[idx++]);
        active_max = float.Parse(data[idx++]);
        active_time_min = float.Parse(data[idx++]);
        active_time_max = float.Parse(data[idx++]);
        passive_min = float.Parse(data[idx++]);
        passive_max = float.Parse(data[idx++]);
        passive_time_min = float.Parse(data[idx++]);
        passive_time_max = float.Parse(data[idx++]);
        used_min = float.Parse(data[idx++]);
        used_max = float.Parse(data[idx++]);
        used_sub = float.Parse(data[idx++]);
        use_min = float.Parse(data[idx++]);
        use_max = float.Parse(data[idx++]);
        use_sub = float.Parse(data[idx++]);
    }

    public float GetActiveSkillValue(int skillLevel)
    {
        return ((active_max - active_min) / 5 * skillLevel) + active_min;
    }
}