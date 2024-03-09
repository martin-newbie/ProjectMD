using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUnitInfo : MonoBehaviour
{
    public SkeletonGraphic model;

    public Image prevExpGauge;
    public Image nextExpGauge;

    public Text prevLevelText;
    public Text nextLevelText;
    public Text updatedExpText;

    public Text flavorText;

    public void InitUnitInfo(UnitData unitData)
    {
        var modelIndex = StaticDataManager.GetConstUnitData(unitData.index).modelIndex;
        var spineAsset = ResourceManager.GetSkeleton(modelIndex);
        model.UpdateSkeleton(spineAsset);

    }
}
