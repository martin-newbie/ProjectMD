using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackSceneButton : MonoBehaviour
{
    public void OnBackScene()
    {
        SceneLoadManager.Instance.LoadBackScene();
    }
}
