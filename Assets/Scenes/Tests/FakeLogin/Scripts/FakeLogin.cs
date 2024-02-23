using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FakeLogin : MonoBehaviour
{
    public InputField idInput;


    public void OnStartButton()
    {
        TestInitUserData();
    }

    void TestInitUserData()
    {
        var nickName = idInput.text;
        WebRequest.Post("user/login", nickName, (data) =>
        {
            var login = JsonUtility.FromJson<LoginPost>(data);
            if (login.isError)
            {
                // play error image
            }
            else
            {
                UserData.Instance = login.userData;
                SceneManager.LoadScene("Main");
            }
        });
    }
}

public class LoginPost
{
    public bool isError;
    public UserData userData;
}