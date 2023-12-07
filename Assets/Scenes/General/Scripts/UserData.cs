using System.Collections.Generic;

[System.Serializable]
public class UserData
{
    public static UserData Instance = null;

    public int playerLevel;
    public float currentExp;

    public string userName;

    public List<UnitData> unitDatas;

    public UserData()
    {
        Instance = this;
        unitDatas = new List<UnitData>();
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
