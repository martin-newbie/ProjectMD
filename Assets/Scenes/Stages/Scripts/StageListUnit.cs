using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageListUnit : MonoBehaviour
{
    int index;
    StageListManager manager;

    [SerializeField] Text stageIndexText;
    [SerializeField] Text stageTitleText;

    public void InitUnit(StageListManager _manager, int idx)
    {
        manager = _manager;
        index = idx;
    }

    public void UpdateUnit(StageData stage)
    {
        stageIndexText.text = stage.chapterIndex.ToString() + "-" + stage.stageIndex.ToString();
    }

    public void OnButtonClick()
    {
        // use index
        manager.ShowStageInfo(index);
    }
}
