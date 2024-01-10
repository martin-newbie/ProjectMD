using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class UserData
{
    public static UserData Instance = null;

    public int userLevel;
    public float currentExp;
    public string userName;

    public List<UnitData> unitDatas;
    public DeckData[] decks;

    public UserData()
    {
        Instance = this;
        unitDatas = new List<UnitData>();
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
    public int unitIdx;
    public int level;
    public float exp;

    public int[] equipmentLevel;
    public float[] equipmentExp;

    public UnitData(int idx)
    {
        unitIdx = idx;
        equipmentLevel = new int[3];
        equipmentExp = new float[3];
    }
}

[System.Serializable]
public class DeckData
{
    public int[] unitsIdx;
}