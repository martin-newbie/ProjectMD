using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
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

    //public GameObject signinGo;
    //public GameObject joinGo;
    //public Text txtUID;
    //public Text txtNickName;
    //public Text txtSuccessLogin;
    //public Button btn;
    //public Button btnSubmit; //��������
    //public InputField inputField;

    //public Button btnSignin;
    //public InputField signinInputField;

    //private string uid;


    //// Start is called before the first frame update
    //#region VARIABLES

    //[Header("Database Properties")]
    //public string Host = "localhost";
    //public string User = "root";
    //public string Password = "root";
    //public string Database = "test";

    //#endregion

    //private void Start()
    //{
    //    Connect();
    //}

    //private void Connect()
    //{
    //    MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
    //    builder.Server = Host;
    //    builder.UserID = User;
    //    builder.Password = Password;
    //    builder.Database = Database;

    //    try
    //    {
    //        using (MySqlConnection connection = new MySqlConnection(builder.ToString()))
    //        {
    //            connection.Open();
    //            print("MySQL - Opened Connection");
    //        }
    //    }
    //    catch (MySqlException exception)
    //    {
    //        print(exception.Message);
    //    }
    //}

 
}
      

