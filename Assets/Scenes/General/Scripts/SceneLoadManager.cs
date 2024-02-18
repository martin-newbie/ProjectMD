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

    public void LoadHomeScene()
    {
        moveSceneName = "Main";
        previousScenes.Clear();
        StartCoroutine(LoadSceneAsync());
    }

    public void LoadBackScene()
    {
        moveSceneName = previousScenes.Pop();
        StartCoroutine(LoadSceneAsync());
    }

    public void LoadScene(string sceneName)
    {
        moveSceneName = sceneName;
        previousScenes.Push(SceneManager.GetActiveScene().name);
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
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
        yield break;
    }
}
