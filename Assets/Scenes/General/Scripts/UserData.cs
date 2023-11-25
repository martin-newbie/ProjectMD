using System.Collections.Generic;

[System.Serializable]
public class UserData
{
    public int playerLevel;
    public float currentExp;

    public string userName;

    public List<CharacterData> charDatas;

    public UserData()
    {
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

    public CharacterData()
    {
        equipmentLevel = new int[3];
        equipmentExp = new float[3];
    }
}
