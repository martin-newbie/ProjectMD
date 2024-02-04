using System.Collections.Generic;
using System.Linq;

[System.Serializable]
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

    public List<UnitData> units;

    public UserData()
    {
        Instance = this;
    }
}

[System.Serializable]
public class UnitData
{
    public int id;
    public int index;
    public int level;
    public float exp;
    public string user_uuid;

    public int[] skill_level;
}

[System.Serializable]
public class DeckData
{
    public int id;
    public int deck_index;
    public string title;
    public string user_uuid;

    public int[] unit_indexes;
}