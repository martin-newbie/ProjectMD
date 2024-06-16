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
    
    // 다른 스킬에 의해 체인될 때
    public float chain_use_min;
    public float chain_use_max;
    public float chain_use_sub;
    
    public float chain_enhance_min;
    public float chain_enhance_max;
    public float chaine_enhance_sub;

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
        chain_use_min = float.Parse(data[idx++]);
        chain_use_max = float.Parse(data[idx++]);
        chain_use_sub = float.Parse(data[idx++]);
        chain_enhance_min = float.Parse(data[idx++]);
        chain_enhance_max = float.Parse(data[idx++]);
        chaine_enhance_sub = float.Parse(data[idx++]);
    }

    public float GetActiveValue(int rank)
    {
        return ((active_max - active_min) / 4 * rank) + active_min;
    }

    public float GetActiveTime(int rank)
    {
        return active_time_min + (active_time_max - active_time_min) / 4 * rank;
    }

    public float GetPassiveValue(int rank)
    {
        return passive_min + (passive_max - passive_min) / 4 * rank;
    }

    public float GetPassiveTime(int rank)
    {
        return passive_time_min + (passive_time_max - passive_time_min) / 4 * rank;
    }

    public float GetChainUseValue(int rank)
    {
        return chain_use_min + (chain_use_max - chain_use_min) / 4 * rank;
    }

    public float GetChainEnhanceValue(int rank)
    {
        return chain_enhance_min + (chain_enhance_max - chain_enhance_min) / 4 * rank;
    }
}