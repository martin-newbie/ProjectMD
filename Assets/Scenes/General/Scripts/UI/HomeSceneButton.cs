using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeSceneButton : MonoBehaviour
{
    public void OnHomeButton()
    {
        SceneLoadManager.Instance.LoadHomeScene();
    }
}
