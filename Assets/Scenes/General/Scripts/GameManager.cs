using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        new TempData();
        new UserData();
        TestInitUserData();
    }

    void TestInitUserData()
    {
        UserData user = new UserData();
        for (int i = 0; i < 26; i++)
        {
            user.unitDatas.Add(new UnitData(i));
        }
    }
}
