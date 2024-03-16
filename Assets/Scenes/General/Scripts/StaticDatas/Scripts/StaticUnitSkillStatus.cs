using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "StaticUnitSkillStatus", menuName = "MD/ScriptableData/StaticUnitSkillStatus", order = int.MinValue)]
public class StaticUnitSkillStatus : SheetDataBase
{
    protected override string gid => "1734931170";

    protected override string range => "D4:S29";

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

    public float[] active;
    public float[] passive;
    public float[] enforce;
    public float[] sub;

    public UnitSkillStatus(string[] data)
    {
        int idx = 0;

        unitIndex = int.Parse(data[idx++]);
        cost = int.Parse(data[idx++]);
        skill_type = int.Parse(data[idx++]);

        active = data[idx++].GetSplitCommaFloat();
        passive = data[idx++].GetSplitCommaFloat();
        enforce = data[idx++].GetSplitCommaFloat();
        sub = data[idx++].GetSplitCommaFloat();
    }

    public float GetActiveSkillValue(int skillLevel)
    {
        return ((active[1] - active[0]) / 5 * skillLevel) + active[0];
    }

    public float GetPassiveSkillValue(int skillLevel)
    {
        return ((passive[1] - passive[0]) / 10 * skillLevel) + passive[0];
    }

    public float GetEnforceSkillValue(int skillLevel)
    {
        return ((enforce[1] - enforce[0]) / 10 * skillLevel) + enforce[0];
    }
    
    public float GetSubSKillValue(int skillLevel)
    {
        return ((sub[1] - sub[0]) / 10 * skillLevel) + sub[0];   
    }
}