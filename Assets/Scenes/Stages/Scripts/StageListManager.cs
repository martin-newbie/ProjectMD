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
        unitList = new List<StageListUnit>();
        int allocCount = 20;
        for (int i = 0; i < allocCount; i++)
        {
            var unit = Instantiate(stageUnitPrefab, stageContent);
            unit.InitUnit(i);
            unitList.Add(unit);
        }

        chapterIndex = 0;
        var chapterData = StageManager.Instance.GetChapterData(chapterIndex);
        for (int i = 0; i < unitList.Count; i++)
        {
            if (i < chapterData.stageDatas.Count)
            {
                unitList[i].gameObject.SetActive(true);
            }
            else
            {
                unitList[i].gameObject.SetActive(false);
            }
        }
    }

    public void ShowStageInfo(int stageIdx)
    {
        var stageData = StageManager.Instance.GetStageData(chapterIndex, stageIdx);
        stageInfo.OpenStageInfo(stageData);
    }

}
