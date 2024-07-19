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
    [SerializeField] GameObject[] stageResultStar;

    [SerializeField] Sprite[] chapterButtonSprites;
    [SerializeField] Image buttonImage;
    [SerializeField] Image buttonBlur;


    public void InitUnit(StageListManager _manager)
    {
        manager = _manager;
    }

    public void UpdateStageInfo(int chapter, int stage)
    {
        this.stage = stage;
        this.chapter = chapter;

        activate = UserData.Instance.stage_result.Count + 1 > chapter * 20 + stage;
        buttonImage.sprite = chapterButtonSprites[chapter];
        buttonBlur.sprite = chapterButtonSprites[chapter];
        buttonBlur.gameObject.SetActive(!activate);

        foreach (var star in stageResultStar)
        {
            star.gameObject.SetActive(false);
        }
    }

    public void UpdateStageResult(StageResult data)
    {
        for (int i = 0; i < data.condition.Length; i++)
        {
            if (data != null) stageResultStar[i].SetActive(data.condition[i]);
            else stageResultStar[i].SetActive(false);
        }
    }

    public void OnStartButton()
    {
        StageListManager.Instance.OnGameStart(stage);
    }

    public void OnInfoButton()
    {
        if (!activate) return;

        // use index
        manager.ShowStageInfo(stage);
    }
}
