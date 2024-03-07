using System;
using System.Collections.Generic;

[Serializable]
public class UnitData
{
    public int id;
    public int index;
    public int rank;
    public int level;
    public float exp;
    public string user_uuid;

    public int[] skill_level;

    public void UpdateExp(float extra)
    {
        exp += extra;
        // TODO : update level also
    }

    public Dictionary<StatusType, float> GetStatus()
    {
        var defaultStatus = StaticDataManager.GetUnitStatus(index).GetCalculatedValueDictionary(level, rank);
        return defaultStatus;
    }
}