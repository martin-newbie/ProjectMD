using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUnitInfo : MonoBehaviour
{
    [SerializeField] SkeletonGraphic model;
    [SerializeField] Image prevExpGauge;
    [SerializeField] Image nextExpGauge;
    [SerializeField] Text prevLevelText;
    [SerializeField] Text nextLevelText;
    [SerializeField] Text updatedExpText;
    [SerializeField] Text flavorText;

    public void InitUnitInfo(UnitData unitData, int prevLevel, int prevExp, int updatedExp)
    {
        var modelIndex = StaticDataManager.GetConstUnitData(unitData.index).modelIndex;
        var spineAsset = ResourceManager.GetSkeleton(modelIndex);
        model.UpdateSkeleton(spineAsset);

        if(prevLevel < unitData.level)
        {
            prevExpGauge.fillAmount = 0f;
        }
        else
        {
            prevExpGauge.fillAmount = (float)prevExp / (float)ResourceManager.GetUnitLevelupExp(prevLevel);
        }

        prevLevelText.text = unitData.level.ToString();
        nextLevelText.text = (unitData.level + 1).ToString();
        nextExpGauge.fillAmount = (float)unitData.exp / (float)ResourceManager.GetUnitLevelupExp(unitData.level);
        updatedExpText.text = updatedExp.ToString();
        updatedExpText.rectTransform.anchoredPosition = new Vector2(225f * nextExpGauge.fillAmount, 20.5f);
        flavorText.text = "";
    }
}
