using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageListUnit : MonoBehaviour
{
    int stage;
    int chapter;
    bool activate;
    StageListManager manager;

    [SerializeField] Text stageIndexText;
    [SerializeField] Text stageTitleText;
    [SerializeField] GameObject deactiveObject;
    [SerializeField] GameObject[] stageResultStar;

    public void InitUnit(StageListManager _manager, StageResult data)
    {
        manager = _manager;
        stage = data.stage_idx;
        chapter = data.chapter_idx;

        activate = UserData.Instance.stage_result.Count <= chapter * 20 + stage;
        deactiveObject.SetActive(!activate);
        for (int i = 0; i < 3; i++)
        {
            stageResultStar[i].SetActive(data.condition[i]);
        }
    }

    public void UpdateUnit(StageData stage)
    {
        stageIndexText.text = stage.chapterIndex.ToString() + "-" + stage.stageIndex.ToString();
    }

    public void OnButtonClick()
    {
        if (!activate) return;

        // use index
        manager.ShowStageInfo(stage);
    }
}
