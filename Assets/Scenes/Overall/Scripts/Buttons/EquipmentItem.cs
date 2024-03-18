using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentItem : MonoBehaviour
{

    [SerializeField] ItemInfo info;
    [SerializeField] Text useCount;
    [SerializeField] GameObject reduceButton;
    [SerializeField] GameObject selectOutline;

    EquipmentLevelUpgrade itemPanel;

    [HideInInspector] public ItemData linkedData;
    [HideInInspector] public int selectCount;

    public void Init(EquipmentLevelUpgrade panel, ItemData data)
    {
        if (data == null || data.count == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        linkedData = data;
        itemPanel = panel;
        info.InitInfo(data.idx);
        UpdateInterface();
    }

    public void RaiseValue()
    {
        if (selectCount >= linkedData.count) return;
        if (itemPanel.NoMoreItem()) return;

        var value = StaticDataManager.GetItemValueData(linkedData.idx);
        itemPanel.RaiseExp(value.value);
        selectCount++;
        UpdateInterface();
    }

    public void ReduceValue()
    {
        if (selectCount <= 0) return;

        var value = StaticDataManager.GetItemValueData(linkedData.idx);
        itemPanel.ReduceExp(value.value);
        selectCount--;
        UpdateInterface();
    }

    public void ClearSelect()
    {
        selectCount = 0;
        UpdateInterface();
    }

    void UpdateInterface()
    {
        selectOutline.SetActive(selectCount != 0);
        reduceButton.SetActive(selectCount != 0);
        useCount.text = selectCount.ToString();
    }
}
