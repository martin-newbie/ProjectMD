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

    [SerializeField] Text stageTitleText;
    [SerializeField] GameObject deactiveObject;
    [SerializeField] GameObject[] stageResultStar;

    public void InitUnit(StageListManager _manager, int chapter, int stage, StageResult data)
    {
        manager = _manager;
        this.stage = stage;
        this.chapter = chapter;

        activate = UserData.Instance.stage_result.Count + 1 > chapter * 20 + stage;
        deactiveObject.SetActive(!activate);

        for (int i = 0; i < 3; i++)
        {
            if (data != null)
                stageResultStar[i].SetActive(data.condition[i]);
            else stageResultStar[i].SetActive(false);
        }
    }

    public void UpdateUnit(StageData stage)
    {
    }

    public void OnButtonClick()
    {
        if (!activate) return;

        // use index
        manager.ShowStageInfo(stage);
    }
}
