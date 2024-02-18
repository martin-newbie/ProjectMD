using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfoWindow : MonoBehaviour
{
    StageData linkedData;

    public void OpenStageInfo(StageData _linkedData)
    {
        gameObject.SetActive(true);
        linkedData = _linkedData;
    }

    public void OnStartButton()
    {

    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }
}
