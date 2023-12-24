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
    public List<int[]> allDeckUnits;

    public UserData()
    {
        Instance = this;
        unitDatas = new List<UnitData>();
        allDeckUnits = new List<int[]>();
        for (int i = 0; i < 4; i++)
        {
            allDeckUnits.Add(new int[0]);
        }
    }

    public void SetDeckUnitAt(int[] units, int show)
    {
        Instance.allDeckUnits[show] = units;
    }

    public bool AlreadySelected(int unitId)
    {
        for (int i = 0; i < allDeckUnits.Count; i++)
        {
            var deck = allDeckUnits[i].ToList();
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
