using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultCanvas : MonoBehaviour
{
    public ResultUnitInfo[] unitInfos;

    public void ShowResult(RecieveStageResultData resultData)
    {
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
            
        }
    }

    public void OnButtonToList()
    {

    }

    public void OnButtonToHome()
    {

    }

    public void OnButtonToNextStage()
    {

    }
}
