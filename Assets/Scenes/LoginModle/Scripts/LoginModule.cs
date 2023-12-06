using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

#region ���뱸��ü
public class packet
{
    public int cmd;
}
#endregion

//�α��� �� ���� ��ü
public class res_sign : packet
{
    public int errorno;
}

//�α��� �� �����ϴ� ��ü
public class req_sign : packet
{
    public string uid;
}

//ȸ������ �� ���� ��ü
public class res_join : packet
{
    public int errorno;
}

//ȸ�����Խ� �����ϴ� ��ü
public class req_join : packet
{
    public string uid;
    public string nickname;
}

public class LoginModule : MonoBehaviour
{

    public GameObject signinGo;
    public GameObject joinGo;
    public Text txtUID;
    public Text txtNickName;
    public Text txtSuccessLogin;
    public Button btn;
    public Button btnSubmit; //��������
    public InputField inputField;

    public Button btnSignin;
    public InputField signinInputField;

    private string uid;


    // Start is called before the first frame update
    void Start()
    {
        //������ ��û (device�� uid ��Ͽ��� Ȯ��)

        this.Init();

        //��ư �̺�Ʈ ���
        this.btn.onClick.AddListener(() =>
        {
            if (string.IsNullOrEmpty(this.inputField.text))
            {
                Debug.Log("�г����� �Է����ּ���.");
            }
            else
            {
                this.txtNickName.text = this.inputField.text;

                this.inputField.gameObject.SetActive(false);
                this.btn.gameObject.SetActive(false);
                this.txtNickName.gameObject.SetActive(true);
                this.btnSubmit.gameObject.SetActive(true);
            }
        });
        this.btnSignin.onClick.AddListener(() =>
        {
            Debug.Log("�α��� ��ư����");
            this.SignIn((success) =>
            {
                if (success)
                {
                    this.txtSuccessLogin.gameObject.SetActive(true);
                }
            });
        });
        this.btnSubmit.onClick.AddListener(() =>
        {
            var reqJoin = new req_join();
            reqJoin.cmd = 1000;
            reqJoin.uid = this.txtUID.text;
            reqJoin.nickname = this.txtNickName.text;

            var json = JsonUtility.ToJson(reqJoin);
            Debug.Log(json);

            StartCoroutine(this.Post("api/join", json, (result) =>
            {
                //���� 
                var responseResult = JsonUtility.FromJson<res_join>(result);
                Debug.Log(responseResult);
                if (responseResult.cmd == 200)
                {
                    this.joinGo.SetActive(false);
                    this.signinGo.SetActive(true);
                }
                else
                {
                    if (responseResult.errorno == 1062)
                    {
                        Debug.Log("�̹� ȸ����ϵǾ����ϴ�.");
                    }
                }
            }));

        });
    }

    private void Init()
    {
        this.uid = SystemInfo.deviceUniqueIdentifier;
        this.txtUID.text = this.uid;
        this.btnSubmit.gameObject.SetActive(false);
        this.txtNickName.gameObject.SetActive(false);
        this.SignIn((success) =>
        {
            if (success == false)
            {
                this.joinGo.SetActive(true);
            }
            else
            {
                this.txtSuccessLogin.gameObject.SetActive(true);
            }
        });
    }

    private void SignIn(System.Action<bool> OnComplete)
    {
        var reqSignin = new req_sign();
        reqSignin.cmd = 1100;
        reqSignin.uid = this.uid;
        var json = JsonUtility.ToJson(reqSignin);

        StartCoroutine(this.Post("api/signin", json, (result) =>
        {
            //���� 
            var responseResult = JsonUtility.FromJson<res_sign>(result);
            Debug.Log(responseResult);
            Debug.LogFormat("<color=red>{0}</color>", responseResult.cmd);

            if (responseResult.cmd == 200)
            {
                Debug.Log("�α��� ����");
                OnComplete(true);
            }
            if (responseResult.errorno == 9001)
            {
                Debug.Log("ȸ������� ���� ���� ���̵��Դϴ�.");
                OnComplete(false);
            }
        }));
    }

    private string serverPath = "http://127.0.0.1:3001";

    private IEnumerator Post(string uri, string data, Action<string> onResponse)
    {
        var url = string.Format("{0}/{1}", this.serverPath, uri);
        Debug.Log(url);
        Debug.Log(data);

        var req = new UnityWebRequest(url, "POST");
        byte[] body = Encoding.UTF8.GetBytes(data);
        Debug.Log(body);

        req.uploadHandler = new UploadHandlerRaw(body);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        onResponse(req.downloadHandler.text);
    }
}
      

