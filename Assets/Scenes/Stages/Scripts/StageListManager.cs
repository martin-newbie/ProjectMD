using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageListManager : MonoBehaviour
{

    [Header("Stage List")]
    public StageListUnit stageUnitPrefab;
    public Transform stageContent;
    List<StageListUnit> unitList;

    [Header("Stage Info")]
    public StageInfoWindow stageInfo;

    int chapterIndex;

    void Start()
    {
        chapterIndex = 0;

        unitList = new List<StageListUnit>();
        int allocCount = 20;
        var stageDataList = UserData.Instance.stage_result;
        for (int i = 0; i < allocCount; i++)
        {
            var unit = Instantiate(stageUnitPrefab, stageContent);
            unit.InitUnit(this, stageDataList[chapterIndex * 20 + i]);
            unitList.Add(unit);
        }

        UpdateChapterList();
    }

    public void ShowStageInfo(int stageIdx)
    {
        stageInfo.OpenStageInfo(chapterIndex, stageIdx);
    }

    void UpdateChapterList()
    {
        var chapterData = StageManager.Instance.GetChapterData(chapterIndex);
        for (int i = 0; i < unitList.Count; i++)
        {
            if (i < chapterData.stageDatas.Count)
            {
                unitList[i].UpdateUnit(chapterData.stageDatas[i]);
                unitList[i].gameObject.SetActive(true);
            }
            else
            {
                unitList[i].gameObject.SetActive(false);
            }
        }
    }

}
