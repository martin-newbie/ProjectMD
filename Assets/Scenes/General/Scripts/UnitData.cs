using System;
using System.Collections.Generic;
using System.Linq;

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
    public List<EquipmentData> equipments;

    int[] skillUnlockRank = new int[4] { 0, 0, 1, 2 };
    int[] equipmentUnlockLevel = new int[3] { 0, 5, 10 };

    public void UpdateExp(int extra)
    {
        exp += extra;
        while (exp >= DataManager.GetUnitLevelExp(level))
        {
            exp -= DataManager.GetUnitLevelExp(level);
            level++;
        }
    }

    public Dictionary<StatusType, float> GetStatus()
    {
        var defaultStatus = StaticDataManager.GetUnitStatus(index).GetCalculatedValueDictionary(level, rank);

        // get equipment's values
        for (int i = 0; i < equipments.Count; i++)
        {
            var e = equipments[i];
            var valueData = StaticDataManager.GetEquipmentValueData(e.index, e.tier);
            for (int j = 0; j < valueData.buff_type.Length; j++)
            {
                var buffType = valueData.buff_type[j];
                var valueType = valueData.value_type[j];
                var value = valueData.GetLevelBuff(e.level, j);
                if (valueType == 0)
                {
                    defaultStatus[(StatusType)buffType] += value;
                }
                else
                {
                    defaultStatus[(StatusType)buffType] *= 1 + value;
                }
            }
        }

        return defaultStatus;
    }

    public int GetSlotEquipmentIndex(int slot)
    {
        var unitData = StaticDataManager.GetConstUnitData(index);
        return unitData.equipmentIndex[slot];
    }

    public EquipmentData AddEquipmentAt(int slot)
    {
        var equipment = new EquipmentData();
        var unitData = StaticDataManager.GetConstUnitData(index);
        equipment.place_index = slot;
        equipment.index = unitData.equipmentIndex[slot];
        equipments.Add(equipment);
        return equipment;
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
    public int unit_id;

    public void UpdateExp(int extra)
    {
        exp += extra;
        while (exp >= DataManager.GetEquipmentLevelExp(level))
        {
            exp -= DataManager.GetEquipmentLevelExp(level);
            level++;
        }
    }

    public void UpgradeTier()
    {
        tier++;
        exp = 0;
        level = 0;
    }
}