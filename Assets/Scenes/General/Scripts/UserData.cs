using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class UserData
{
    public static UserData Instance = null;

    public int id;
    public string uuid;
    public string nickname;
    public int level;
    public float exp;
    public int dia;
    public int coin;
    public DateTime last_energy_updated;
    public List<UnitData> units;

    public UserData()
    {
        Instance = this;
    }

    public UnitData FindUnitWithId(int id)
    {
        return units.Find(item => item.id == id);
    }

    public void UpdateExp(float extra)
    {
        exp += extra;
    }
}

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
    }

    public Dictionary<StatusType, float> GetStatus()
    {
        var defaultStatus = StaticDataManager.GetUnitStatus(index).GetCalculatedValueDictionary(level, rank);
        return defaultStatus;
    }
}

[Serializable]
public class DeckData
{
    public int id;
    public int deck_index;
    public string title;
    public string user_uuid;

    public int[] unit_indexes;
}