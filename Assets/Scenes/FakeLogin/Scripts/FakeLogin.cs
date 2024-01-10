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
        if (string.IsNullOrEmpty(idInput.text))
        {
            // no account login
            SceneManager.LoadScene("Menu");
            
        }
        else
        {
            WebRequest.Instance.PostWebRequest("Temp_Project_Login", "121212", (data) =>
            {
                var login = JsonUtility.FromJson<LoginPost>(data);
                if (login.isError)
                {
                    // play error image
                }
                else
                {
                    UserData.Instance = login.userData;
                    SceneManager.LoadScene("Menu");
                }
            });
        }
    }
}

public class LoginPost
{
    public bool isError;
    public UserData userData;
}