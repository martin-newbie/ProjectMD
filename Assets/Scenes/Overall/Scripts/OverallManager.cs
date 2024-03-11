using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverallManager : MonoBehaviour
{
    [SerializeField] OverallStatusPanel statusPanel;
    [SerializeField] SkeletonGraphic unitModel;

    private void Start()
    {
        int unitId = TempData.Instance.selectedUnit;
        var unitData = UserData.Instance.FindUnitWithId(unitId);

        unitModel.UpdateSkeleton(ResourceManager.GetSkeleton(unitData.index));
        unitModel.AnimationState.SetAnimation(0, "battle_wait", true);

        statusPanel.InitCharacter(unitData);
    }
}
