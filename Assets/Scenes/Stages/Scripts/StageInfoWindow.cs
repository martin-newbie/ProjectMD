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
        TempData.Instance.selectedChapter = linkedData.chapterIndex;
        TempData.Instance.selectedStage = linkedData.stageIndex;
        SceneLoadManager.Instance.LoadScene("Loadout");
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }
}
