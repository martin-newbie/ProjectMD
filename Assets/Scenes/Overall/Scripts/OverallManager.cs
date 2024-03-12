using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverallManager : MonoBehaviour
{

    public static OverallManager Instance = null;
    private void Awake()
    {
        Instance = this;
    }

    public OverallStatusPanel statusPanel;
    public SkeletonGraphic unitModel;
    public SkillUpgradePanel skillUpgradePanel;

    private void Start()
    {
        int unitId = TempData.Instance.selectedUnit;
        var unitData = UserData.Instance.FindUnitWithId(unitId);

        unitModel.UpdateSkeleton(ResourceManager.GetSkeleton(unitData.index));
        unitModel.AnimationState.SetAnimation(0, "battle_wait", true);

        statusPanel.InitCharacter(unitData);
    }

    public void OpenSkillUpgradePanel(UnitData linkedData, int selectIndex)
    {
        skillUpgradePanel.OpenSkillUpgradePanel(linkedData, selectIndex);
    }
}
