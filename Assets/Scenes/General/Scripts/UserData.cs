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
    public DeckData[] decks;

    public UserData()
    {
        Instance = this;

        decks = new DeckData[4];
        for (int i = 0; i < decks.Length; i++)
        {
            decks[i] = new DeckData();
        }
    }

    public void SetDeckUnitAt(int[] units, int show)
    {
        decks[show].unitsIdx = units;
    }

    public bool AlreadySelected(int unitId)
    {
        for (int i = 0; i < decks.Length; i++)
        {
            var deck = decks[i].unitsIdx;
            if (deck.Contains(unitId))
            {
                return true;
            }
        }
        return false;
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
}

[System.Serializable]
public class DeckData
{
    public int[] unitsIdx;

    public DeckData()
    {
        unitsIdx = new int[0];
    }
}