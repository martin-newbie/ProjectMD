using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverallPanelStatus : MonoBehaviour, OverallPanel
{
    [SerializeField] Text atkText;
    [SerializeField] Text hpText;
    [SerializeField] Text defText;

    [SerializeField] EquipmentInfoButton[] equipmentButtons;
    [SerializeField] Text[] equipmentDescText;

    public void Open(UnitData data)
    {
        var statusData = data.GetStatus();

        atkText.text = statusData[StatusType.DMG].ToString();
        hpText.text = statusData[StatusType.HP].ToString();
        defText.text = statusData[StatusType.DEF].ToString();

        for (int i = 0; i < equipmentButtons.Length; i++)
        {
            var equipment = i < data.equipments.Length ? data.equipments[i] : null;
            equipmentButtons[i].Init(equipment, data.IsEquipmentUnlock(i), ShowEquipmentUpgrade);
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
        throw new System.Exception();
    }
}
