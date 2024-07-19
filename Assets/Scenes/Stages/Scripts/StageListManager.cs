using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class StageListManager : MonoBehaviour
{
    public static StageListManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    [Header("Stage List")]
    public StageListUnit stageUnitPrefab;
    public Transform stageContent;
    public ScrollRect scroll;
    public List<StageListUnit> unitList;

    [Header("Stage Info")]
    public StageInfoWindow stageInfo;

    int chapterIndex;
    int activeStageCount;

    int curStageIdx;
    bool isDragging;
    float targetValue;

    void Start()
    {
        foreach (StageListUnit unit in unitList)
        {
            unit.InitUnit(this);
        }

        UpdateStagesData(0);
    }

    public void UpdateStagesData(int chapter)
    {
        chapterIndex = chapter;
        activeStageCount = 0;
        var resultList = UserData.Instance.stage_result;
        var chapterData = StageManager.Instance.GetChapterData(chapter);

        for (int i = 0; i < unitList.Count; i++)
        {
            if (i < chapterData.stageDatas.Count)
            {
                activeStageCount++;
                unitList[i].gameObject.SetActive(true);
                unitList[i].UpdateStageInfo(chapter, i);
                if(i < resultList.Count)
                {
                    unitList[i].UpdateStageResult(resultList[i]);
                }
            }
            else
            {
                unitList[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnGameStart(int stageIdx)
    {
        GameStart(chapterIndex, stageIdx);
    }

    public void ShowStageInfo(int stageIdx)
    {
        stageInfo.OpenStageInfo(chapterIndex, stageIdx);
    }

    public void GameStart(int chapter, int stage)
    {
        if (UserData.Instance.energy < 10) return; // TODO : replace it to stage data related energy cost

        var sendData = new SendUserData();
        sendData.uuid = UserData.Instance.uuid;
        TempData.Instance.selectedChapter = chapter;
        TempData.Instance.selectedStage = stage;

        WebRequest.Post("main-menu/enter-loadout", JsonUtility.ToJson(sendData), (data) =>
        {
            var recieveData = JsonUtility.FromJson<RecieveDeckData>(data);
            SceneLoadManager.Instance.LoadScene("Loadout", () => { LoadoutManager.Instance.InitLoadout(recieveData); });
        });
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

        if (targetIdx <= 0 || targetIdx >= activeStageCount) targetIdx = Mathf.Clamp(targetIdx, 0, activeStageCount - 1);

        curStageIdx = targetIdx;
        targetValue = curStageIdx * GetDistance();
    }

    float GetDistance()
    {
        return (1f / ((float)activeStageCount - 1));
    }
}
