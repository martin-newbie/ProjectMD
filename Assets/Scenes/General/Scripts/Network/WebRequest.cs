using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequest : MonoBehaviour
{
    public static WebRequest Instance = null;
    private void Awake()
    {
        Instance = this;
        StartCoroutine(SendQueue());
    }

    public bool useAsLocalhost = false;

    public static string portURL = "http://ksmin.iptime.org:8081/rest/";
    public static string testURL = "http://localhost:8083/rest/";

    public static void Get(string url, string[] bodyParamKeys, string[] bodyParamValues, Action<string> endAction = null)
    {
        var request = new WebRequestData();

        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < bodyParamKeys.Length; i++)
        {
            builder.Append(bodyParamKeys[i] + "=" + bodyParamValues[i]);
        }
        string paramsUrl = url + "?" + builder.ToString();

        request.sendUrl = paramsUrl;
        request.endAction = endAction;
        request.request = (url, form) => { return UnityWebRequest.Get(url); };

        Instance.requestQueue.Enqueue(request);
    }

    public static void Post(string url, string data, Action<string> endAction = null)
    {
        var request = new WebRequestData();
        var form = new WWWForm();

        // add id in form
        form.AddField("input_data", data);

        request.sendUrl = url;
        request.form = form;
        request.endAction = endAction;
        request.request = (url, form) => { return UnityWebRequest.Post(url, form); };

        Instance.requestQueue.Enqueue(request);
    }

    Queue<WebRequestData> requestQueue = new Queue<WebRequestData>();

    IEnumerator SendQueue()
    {
        while (true)
        {
            yield return new WaitUntil(() => requestQueue.Count > 0);

            var queue = requestQueue.Dequeue();
            using (UnityWebRequest request = queue.request(GetSendUri() + queue.sendUrl, queue.form))
            {
                Debug.Log(request.url);
                yield return request.SendWebRequest();
                if (!string.IsNullOrEmpty(request.error))
                {
                    Debug.Log(request.error);
                    continue;
                }

                try
                {
                    Debug.Log(request.downloadHandler.text);
                    queue.endAction?.Invoke(request.downloadHandler.text);
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.Message);
                    // 재접속 유도
                    throw;
                }
            }
        }
    }

    string GetSendUri()
    {
        return useAsLocalhost ? testURL : portURL;
    }
}

public class WebRequestData
{
    public string sendUrl;
    public WWWForm form;
    public Action<string> endAction;
    public Func<string, WWWForm, UnityWebRequest> request;
}