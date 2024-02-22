using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager Instance;
    public Stack<string> previousScenes = new Stack<string>();

    string moveSceneName;

    private void Awake()
    {
        Instance = this;
    }

    public void LoadHomeScene(Action onComplete = null)
    {
        moveSceneName = "Main";
        previousScenes.Clear();
        StartCoroutine(LoadSceneAsync(onComplete));
    }

    public void LoadBackScene(Action onComplete = null)
    {
        moveSceneName = previousScenes.Pop();
        StartCoroutine(LoadSceneAsync(onComplete));
    }

    public void LoadScene(string sceneName, Action onComplete = null)
    {
        moveSceneName = sceneName;
        previousScenes.Push(SceneManager.GetActiveScene().name);
        StartCoroutine(LoadSceneAsync(onComplete));
    }

    IEnumerator LoadSceneAsync(Action onComplete)
    {
        var loadData = SceneManager.LoadSceneAsync(moveSceneName);
        loadData.allowSceneActivation = false;
        while (!loadData.isDone)
        {
            // scene progress
            float progress = loadData.progress;
            if(progress >= 0.9f)
            {
                break;
            }
            yield return null;
        }

        loadData.allowSceneActivation = true;
        yield return null;
        onComplete?.Invoke();

        yield break;
    }
}
