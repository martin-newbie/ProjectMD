using System;
using System.Collections.Generic;

[Serializable]
public class UnitData
{
    public int id;
    public int index;
    public int rank;
    public int level;
    public int exp;
    public string user_uuid;

    public int[] skill_level;
    public EquipmentData[] equipments;

    public void UpdateExp(int extra)
    {
        exp += extra;
        while (exp > ResourceManager.GetUnitLevelupExp(level))
        {
            exp -= ResourceManager.GetUnitLevelupExp(level);
            level++;
        }
    }

    public Dictionary<StatusType, float> GetStatus()
    {
        var defaultStatus = StaticDataManager.GetUnitStatus(index).GetCalculatedValueDictionary(level, rank);
        return defaultStatus;
    }
}

[Serializable]
public class EquipmentData
{
    public int index;
    public int place_index;
    public int level;
    public int tier;
    public int exp;
}