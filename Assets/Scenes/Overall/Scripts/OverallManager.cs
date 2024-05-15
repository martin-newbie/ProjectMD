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

    public SkeletonGraphic unitModel;

    private void Start()
    {
        int unitId = TempData.Instance.selectedUnit;
        var unitData = UserData.Instance.FindUnitWithId(unitId);

        unitModel.UpdateSkeleton(ResourceManager.GetSkeleton(unitData.index));
        unitModel.AnimationState.SetAnimation(0, "battle_wait", true);
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

    public int[] GetAutoFillCount(int[] itemIndex, int totalExp)
    {
        int[] result = new int[itemIndex.Length];
        for (int i = 0; i < itemIndex.Length; i++)
        {
            int index = itemIndex[i];
            int count = UserData.Instance.FindItemCount(index);
            int value = StaticDataManager.GetItemValueData(index).value;
            int totalCount = Mathf.Min(totalExp / value, count);
            if (totalCount < 0) break;

            if (count > totalCount && i != itemIndex.Length - 1)
            {
                int extraExp = 0;
                for (int j = i + 1; j < itemIndex.Length; j++)
                {
                    int lowValue = StaticDataManager.GetItemValueData(index).value * UserData.Instance.FindItemCount(index);
                    extraExp += lowValue;
                }

                if (extraExp < totalExp - totalCount * value)
                {
                    totalCount++;
                }
            }

            if (i == itemIndex.Length - 1 && count > totalCount)
            {
                totalCount++;
            }

            totalExp -= totalCount * value;
            result[i] = totalCount;
        }

        return result;
    }
}
