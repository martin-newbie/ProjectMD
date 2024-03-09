using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultCanvas : MonoBehaviour
{
    public ResultUnitInfo[] unitInfos;
    public ResultRewardInfo rewardPrefab;
    public Transform rewardContent;

    public void ShowResult(RecieveStageResultData resultData)
    {
        gameObject.SetActive(true);
        InitUnitInfo(resultData);
        InitRewardInfo(resultData);
    }

    void InitUnitInfo(RecieveStageResultData resultData)
    {
        for (int i = 0; i < unitInfos.Length; i++)
        {
            if (i < resultData.units.Length)
            {
                var unit = UserData.Instance.FindUnitWithId(resultData.units[i]);
                int prevLevel = unit.level;
                int prevExp = unit.exp;
                unit.UpdateExp(resultData.exp);
                unitInfos[i].InitUnitInfo(unit, prevLevel, prevExp, resultData.exp);
            }
            else
            {
                unitInfos[i].gameObject.SetActive(false);
            }
        }
    }

    void InitRewardInfo(RecieveStageResultData resultData)
    {
        foreach (var reward in resultData.reward)
        {
            var icon = Instantiate(rewardPrefab, rewardContent);
            icon.InitRewardInfo(reward);
        }
    }

    public void OnButtonToList()
    {
        SceneLoadManager.Instance.LoadScene("List");
    }

    public void OnButtonToHome()
    {
        SceneLoadManager.Instance.LoadHomeScene();
    }

    public void OnButtonToNextStage()
    {
        // do nothing now
    }
}
