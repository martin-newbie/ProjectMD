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
    }
}
