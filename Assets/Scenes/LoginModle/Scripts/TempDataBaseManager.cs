using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class TempDataBaseManager : MonoBehaviour
{
    /*
    #region VARIABLES

    [Header("Connection Properties")]
    public string Host = "localhost";
    public string Port = "3306";
    public string User = "root";
    public string Password = "root";
    public string Database = "test";

    #endregion

    #region UNITY METHODS

    private void Start()
    {
        Connect();
    }

    #endregion

    #region METHODS

    private void Connect()
    {
        try
        {
            string connectionString = "DRIVER={MySQL ODBC 8.0 Unicode Driver};" +
                                      $"SERVER={Host};" +
                                      $"PORT={Port};" +
                                      $"DATABASE={Database};" +
                                      $"UID={User};" +
                                      $"PWD={Password};";
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                connection.Open();
                print("ODBC - Opened Connection");
            }
        }
        catch (OdbcException exception)
        {
            print(exception.Message);
        }
    }
    #endregion
       */
    //void Start()
    //{
    //    //서버에 요청 (device의 uid 등록여부 확인)

    //    this.Init();

    //    //버튼 이벤트 등록
    //    this.btn.onClick.AddListener(() =>
    //    {
    //        if (string.IsNullOrEmpty(this.inputField.text))
    //        {
    //            Debug.Log("닉네임을 입력해주세요.");
    //        }
    //        else
    //        {
    //            this.txtNickName.text = this.inputField.text;

    //            this.inputField.gameObject.SetActive(false);
    //            this.btn.gameObject.SetActive(false);
    //            this.txtNickName.gameObject.SetActive(true);
    //            this.btnSubmit.gameObject.SetActive(true);
    //        }
    //    });
    //    this.btnSignin.onClick.AddListener(() =>
    //    {
    //        Debug.Log("로그인 버튼누름");
    //        this.SignIn((success) =>
    //        {
    //            if (success)
    //            {
    //                this.txtSuccessLogin.gameObject.SetActive(true);
    //            }
    //        });
    //    });
    //    this.btnSubmit.onClick.AddListener(() =>
    //    {
    //        var reqJoin = new req_join();
    //        reqJoin.cmd = 1000;
    //        reqJoin.uid = this.txtUID.text;
    //        reqJoin.nickname = this.txtNickName.text;

    //        var json = JsonUtility.ToJson(reqJoin);
    //        Debug.Log(json);

    //        StartCoroutine(this.Post("api/join", json, (result) =>
    //        {
    //            //응답 
    //            var responseResult = JsonUtility.FromJson<res_join>(result);
    //            Debug.Log(responseResult);
    //            if (responseResult.cmd == 200)
    //            {
    //                this.joinGo.SetActive(false);
    //                this.signinGo.SetActive(true);
    //            }
    //            else
    //            {
    //                if (responseResult.errorno == 1062)
    //                {
    //                    Debug.Log("이미 회원등록되었습니다.");
    //                }
    //            }
    //        }));

    //    });
    //}

    //private void Init()
    //{
    //    this.uid = SystemInfo.deviceUniqueIdentifier;
    //    this.txtUID.text = this.uid;
    //    this.btnSubmit.gameObject.SetActive(false);
    //    this.txtNickName.gameObject.SetActive(false);
    //    this.SignIn((success) =>
    //    {
    //        if (success == false)
    //        {
    //            this.joinGo.SetActive(true);
    //        }
    //        else
    //        {
    //            this.txtSuccessLogin.gameObject.SetActive(true);
    //        }
    //    });
    //}

    //private void SignIn(System.Action<bool> OnComplete)
    //{
    //    var reqSignin = new req_sign();
    //    reqSignin.cmd = 1100;
    //    reqSignin.uid = this.uid;
    //    var json = JsonUtility.ToJson(reqSignin);

    //    StartCoroutine(this.Post("api/signin", json, (result) =>
    //    {
    //        //응답 
    //        var responseResult = JsonUtility.FromJson<res_sign>(result);
    //        Debug.Log(responseResult);
    //        Debug.LogFormat("<color=red>{0}</color>", responseResult.cmd);

    //        if (responseResult.cmd == 200)
    //        {
    //            Debug.Log("로그인 성공");
    //            OnComplete(true);
    //        }
    //        if (responseResult.errorno == 9001)
    //        {
    //            Debug.Log("회원등록이 되지 않은 아이디입니다.");
    //            OnComplete(false);
    //        }
    //    }));
    //}

    //private string serverPath = "http://127.0.0.1:3001";

    //private IEnumerator Post(string uri, string data, Action<string> onResponse)
    //{
    //    var url = string.Format("{0}/{1}", this.serverPath, uri);
    //    Debug.Log(url);
    //    Debug.Log(data);

    //    var req = new UnityWebRequest(url, "POST");
    //    byte[] body = Encoding.UTF8.GetBytes(data);
    //    Debug.Log(body);

    //    req.uploadHandler = new UploadHandlerRaw(body);
    //    req.downloadHandler = new DownloadHandlerBuffer();
    //    req.SetRequestHeader("Content-Type", "application/json");

    //    yield return req.SendWebRequest();

    //    onResponse(req.downloadHandler.text);
    //}

}

