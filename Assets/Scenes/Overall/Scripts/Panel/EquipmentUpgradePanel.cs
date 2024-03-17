using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUpgradePanel : MonoBehaviour
{
    [SerializeField] Text requireCoinText;
    [SerializeField] EquipmentLevelUpgrade levelPanel;

    public void InitPanel(EquipmentData data)
    {
        gameObject.SetActive(true);
        ShowEquipment(data);
    }

    public void ShowEquipment(EquipmentData data)
    {
        if (data == null) return; // no equipment available

        int maxLevel = 59;
        var staticData = StaticDataManager.GetEquipmentValueData(data.index, data.tier);

        if (data.level == maxLevel)
        {
            // already max level
        }
        else if (data.level >= staticData.max_level)
        {
            // show tier upgrade
        }
        else
        {
            levelPanel.Open(data);
        }
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
        levelPanel.ClearSelectedData();
    }
}
