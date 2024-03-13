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

    int[] skillUnlockRank = new int[4] { 0, 0, 1, 2 };
    int[] equipmentUnlockLevel = new int[3] { 0, 5, 10 };

    public void UpdateExp(int extra)
    {
        exp += extra;
        while (exp > DataManager.GetUnitLevelExp(level))
        {
            exp -= DataManager.GetUnitLevelExp(level);
            level++;
        }
    }

    public Dictionary<StatusType, float> GetStatus()
    {
        var defaultStatus = StaticDataManager.GetUnitStatus(index).GetCalculatedValueDictionary(level, rank);
        return defaultStatus;
    }

    public bool IsSkillLock(int idx)
    {
        return skillUnlockRank[idx] > rank;
    }

    public bool IsEquipmentLock(int idx)
    {
        return equipmentUnlockLevel[idx] > level;
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