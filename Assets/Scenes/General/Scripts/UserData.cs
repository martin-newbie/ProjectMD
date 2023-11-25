using System.Collections.Generic;

[System.Serializable]
public class UserData
{
    public static UserData Instance = null;

    public int playerLevel;
    public float currentExp;

    public string userName;

    public List<CharacterData> charDatas;

    public UserData()
    {
        Instance = this;
        charDatas = new List<CharacterData>();
    }
}

[System.Serializable]
public class CharacterData
{
    public int charIdx;
    public float charExp;

    public int[] equipmentLevel;
    public float[] equipmentExp;

    public CharacterData(int idx)
    {
        charIdx = idx;
        equipmentLevel = new int[3];
        equipmentExp = new float[3];
    }
}
