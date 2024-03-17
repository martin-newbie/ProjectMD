using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUpgradePanel : MonoBehaviour
{
    [SerializeField] EquipmentInfoButton[] listButtons;
    [SerializeField] Text requireCoinText;
    [SerializeField] EquipmentLevelUpgrade levelPanel;

    UnitData linkedUnit;

    public void InitPanel(UnitData unitData, int openIdx)
    {
        linkedUnit = unitData;

        for (int i = 0; i < listButtons.Length; i++)
        {
            listButtons[i].Init(unitData.FindEquipmentAt(i), unitData.IsEquipmentLock(i), ShowEquipment);
        }

    }

    public void ShowEquipment(EquipmentData data)
    {
        if (data == null) return; // no equipment available

        int maxLevel = DataManager.Instance.gameData.max_leve_data.equipment_max_level;
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
}
