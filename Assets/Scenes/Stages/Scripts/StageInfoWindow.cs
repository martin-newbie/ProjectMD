using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfoWindow : MonoBehaviour
{
    public int chapter;
    public int stage;

    public void OpenStageInfo(int chapter, int stage)
    {
        this.chapter = chapter;
        this.stage = stage;
        gameObject.SetActive(true);
    }

    public void OnStartButton()
    {
        StageListManager.Instance.GameStart(chapter, stage);
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }
}
