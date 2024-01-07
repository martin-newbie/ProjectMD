using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class FakeLogin : MonoBehaviour
{

    private void Start()
    {
        WebRequest.Instance.PostWebRequest("Temp_Project_Login", "121212", (data)=> {
            UserData.Instance = JsonUtility.FromJson<UserData>(data);
        });
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
        SceneManager.LoadScene("Menu");
    }
}
