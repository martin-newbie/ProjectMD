using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverallPanelStatus : MonoBehaviour, IOverallPanel
{
    [SerializeField] Text atkText;
    [SerializeField] Text hpText;
    [SerializeField] Text defText;

    [SerializeField] EquipmentInfoButton[] equipmentButtons;
    [SerializeField] Text[] equipmentDescText;

    UnitData unitData;

    public void Open(UnitData data)
    {
        unitData = data;
        var statusData = data.GetStatus();
        atkText.text = OverallManager.Instance.GetStatusText(StatusType.DMG, statusData);
        hpText.text = OverallManager.Instance.GetStatusText(StatusType.HP, statusData);
        defText.text = OverallManager.Instance.GetStatusText(StatusType.DEF, statusData);

        for (int i = 0; i < equipmentButtons.Length; i++)
        {
            var equipment = i < data.equipments.Count ? data.equipments[i] : null;
            equipmentButtons[i].Init(equipment, data.IsEquipmentLock(i), ShowEquipmentUpgrade);
        }

        for (int i = 0; i < equipmentDescText.Length; i++)
        {
            // get description localize text
            string localizeText = "";
            equipmentDescText[i].text = localizeText;
        }
    }

    void ShowEquipmentUpgrade(EquipmentData data)
    {
        if(data == null)
        {

        }
        OverallManager.Instance.OpenEquipmentUpgradePanel(data);
    }
}
