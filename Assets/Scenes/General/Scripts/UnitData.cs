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
        return defaultStatus;
    }
}