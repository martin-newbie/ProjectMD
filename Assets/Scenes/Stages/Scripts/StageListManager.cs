using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageListManager : MonoBehaviour
{

    [Header("Stage List")]
    public StageListUnit stageUnitPrefab;
    public Transform stageContent;
    public ScrollRect scroll;
    public List<StageListUnit> unitList;

    [Header("Stage Info")]
    public StageInfoWindow stageInfo;

    int chapterIndex;

    int curStageIdx;
    bool isDragging;
    float targetValue;

    void Start()
    {
    }

    public void ShowStageInfo(int stageIdx)
    {
        stageInfo.OpenStageInfo(chapterIndex, stageIdx);
    }

    public void OnLeftButton()
    {
        if (curStageIdx > 0)
        {
            curStageIdx--;
            targetValue = curStageIdx * GetDistance();
        }
    }

    public void OnRightButton()
    {
        if (curStageIdx < unitList.Count - 1)
        {
            curStageIdx++;
            targetValue = curStageIdx * GetDistance();
        }
    }

    private void Update()
    {
        if (isDragging)
        {

        }
        else
        {
            scroll.horizontalScrollbar.value = Mathf.Lerp(scroll.horizontalScrollbar.value, targetValue, Time.deltaTime * 15f);
            for (int i = 0; i < unitList.Count; i++)
            {
                var targetScale = i == curStageIdx ? Vector3.one : Vector3.one * 0.9f;
                unitList[i].transform.localScale = Vector3.Lerp(unitList[i].transform.localScale, targetScale, Time.deltaTime * 15f);
            }
        }

    }

    public void OnDragStart()
    {
        isDragging = true;
    }

    public void OnDragFinish()
    {
        isDragging = false;

        int value = (int)(scroll.horizontalScrollbar.value * 100);
        int distance = (int)(GetDistance() * 100);
        int index = 0;
        while (value > distance)
        {
            value -= distance;
            index++;
        }
        int targetIdx = value < (float)distance / 2f ? index : index + 1;

        if (targetIdx <= 0 || targetIdx >= unitList.Count) targetIdx = Mathf.Clamp(targetIdx, 0, unitList.Count - 1);

        curStageIdx = targetIdx;
        targetValue = curStageIdx * GetDistance();
    }

    float GetDistance()
    {
        return (1f / ((float)unitList.Count - 1));
    }
}
