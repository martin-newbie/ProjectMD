using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FakeLogin : MonoBehaviour
{

    private void Start()
    {
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

    public void OnStartButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
