using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverallPanelSummary : MonoBehaviour, IOverallPanel
{
    [Header("Main")]
    public Text unitNameText;
    public Text unitLevelText;
    public GameObject[] tierObjects;

    [Header("Type")]
    public Text atkTypeText;
    public Image atkTypeColor;
    public Text defTypeText;
    public Image defTypeColor;

    [Header("Status")]
    public Text atkStatusText;
    public Text hpStatusText;
    public Text defStatusText;

    [Header("Skills")]
    public SkillInfoButton[] skillButtons;

    [Header("Equipments")]
    public EquipmentInfoButton[] equipmentButtons;


    public void Open(UnitData data)
    {
        var constData = StaticDataManager.GetConstUnitData(data.index);
        var statusData = data.GetStatus();

        unitNameText.text = constData.name;
        unitLevelText.text = "Lv." + data.level.ToString();
        for (int i = 0; i < tierObjects.Length; i++)
        {
            tierObjects[i].SetActive(i <= data.rank);
        }

        atkStatusText.text = statusData[StatusType.DMG].ToString();
        hpStatusText.text = statusData[StatusType.HP].ToString();
        defStatusText.text = statusData[StatusType.DEF].ToString();

        for (int i = 0; i < skillButtons.Length; i++)
        {
            skillButtons[i].Init(i, data.skill_level[i], data.IsSkillUnlock(i), OpenSkillInfo);
        }

        for (int i = 0; i < equipmentButtons.Length; i++)
        {
            var equipment = i < data.equipments.Length ? data.equipments[i] : null;
            equipmentButtons[i].Init(equipment, data.IsEquipmentUnlock(i), OpenEquipmentInfo);
        }
    }

    void OpenSkillInfo(int index, int level)
    {
        throw new Exception();
    }

    void OpenEquipmentInfo(EquipmentData data)
    {
        throw new Exception();
    }
}
