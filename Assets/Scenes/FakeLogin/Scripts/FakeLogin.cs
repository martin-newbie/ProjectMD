using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class FakeLogin : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(uploaduserdata());
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

    IEnumerator uploaduserdata()
    {
        string id = "근섭";
        string pwd = "3333";
        WWWForm form = new WWWForm();
        form.AddField("input_id", id);
        form.AddField("input_pwd", pwd);

        string url = "projectmd.dothome.co.kr/phps/Temp_Project_Login.php";
        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {
            Debug.Log("보냄");
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
            }
            Debug.Log(request.downloadHandler.text);

        }
    }

}
