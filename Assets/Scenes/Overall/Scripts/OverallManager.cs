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
    public EquipmentUpgradePanel equipmentUpgradePanel;

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

    public void OpenEquipmentUpgradePanel(EquipmentData linkedData)
    {
        equipmentUpgradePanel.InitPanel(linkedData);
    }

    public string ModifyUpgradedColorText(float text)
    {
        return "<color=#47E76D> → " + text.ToString("N0") + "</color>";
    }

    public string GetStatusText(StatusType type, Dictionary<StatusType, float> value)
    {
        switch (type)
        {
            case StatusType.DMG:
                return $"ATTACK : {value[StatusType.DMG].ToString("N0")}";
            case StatusType.HP:
                return $"HP : {value[StatusType.HP].ToString("N0")}";
            case StatusType.DEF:
                return $"DEF : {value[StatusType.DEF].ToString("N0")}";
            default:
                return "";

        }
    }

    public bool CheckEnoughtItems(ItemData[] items)
    {
        bool result = true;
        foreach (var item in items)
        {
            var findItem = UserData.Instance.FindItem(item.idx);
            if (findItem.count < item.count)
            {
                result = false;
            }
        }
        return result;
    }

    public int[] GetAutoFillCount(int[] itemIndex, int total)
    {
        for (int i = 0; i < itemIndex.Length; i++)
        {
            var index = itemIndex[itemIndex.Length - i - 1];
            var item = UserData.Instance.FindItemCount(index);
            var value = StaticDataManager.GetItemValueData(index).value;

        }
        return null;
    }
}
