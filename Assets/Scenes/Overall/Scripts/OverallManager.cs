using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverallManager : MonoBehaviour
{
    [SerializeField] OverallStatusPanel statusPanel;
    [SerializeField] SkeletonGraphic unitModel;

    [Header("Summary")]
    [SerializeField] Text nameText;
    [SerializeField] Text positionText;
    [SerializeField] Image positionImage;
    [SerializeField] Text atkTypeText;
    [SerializeField] Image atkTypeImage;
    [SerializeField] Text defTypeText;
    [SerializeField] Image defTypeImage;

    private void Start()
    {
        int unitId = TempData.Instance.selectedUnit;
        var unitData = UserData.Instance.FindUnitWithId(unitId);
        var unitConstData = StaticDataManager.GetConstUnitData(unitData.index);

        nameText.text = unitConstData.name;

        unitModel.Update(0f);
        unitModel.skeletonDataAsset = ResourceManager.GetSkeleton(unitData.index);
        unitModel.Initialize(true);
        unitModel.AnimationState.SetAnimation(0, "battle_wait", false);

        statusPanel.InitCharacter(unitData);
    }
}
