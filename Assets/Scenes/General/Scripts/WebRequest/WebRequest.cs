using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequest : MonoBehaviour
{
    public static WebRequest Instance = null;
    private void Awake()
    {
        Instance = this;
        StartCoroutine(SendQueue());
        DontDestroyOnLoad(gameObject);
    }

    Queue<WebRequestData> requestQueue = new Queue<WebRequestData>();

    public void PostWebRequest(string url, string jsonData, Action<string> endAction = null)
    {
        string id = "근섭"; // TODO: 스태틱으로 저장되어있는 아이디를 넣어줌
        WWWForm form = new WWWForm();
        form.AddField("input_id", id);
        form.AddField("input_data", jsonData);

        string sendUrl = $"projectmd.dothome.co.kr/phps/{url}.php";

        var data = new WebRequestData();
        data.sendUrl = sendUrl;
        data.form = form;
        data.endAction = endAction;
        requestQueue.Enqueue(data);
    }

    IEnumerator SendQueue()
    {
        while (true)
        {
            yield return new WaitUntil(() => requestQueue.Count > 0);
            var queue = requestQueue.Dequeue();
            using (UnityWebRequest request = UnityWebRequest.Post(queue.sendUrl, queue.form))
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
                    queue.endAction?.Invoke(request.downloadHandler.text);
                }
                Debug.Log(request.downloadHandler.text);
            }
        }
    }
}

public class WebRequestData
{
    public string sendUrl;
    public WWWForm form;
    public Action<string> endAction;
}