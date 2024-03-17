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
        unitLevelText.text = "Lv." + (data.level + 1).ToString();
        for (int i = 0; i < tierObjects.Length; i++)
        {
            tierObjects[i].SetActive(i <= data.rank);
        }

        atkStatusText.text = $"ATTACK : {statusData[StatusType.DMG].ToString("N0")}";
        hpStatusText.text = $"HP : {statusData[StatusType.HP].ToString("N0")}";
        defStatusText.text = $"DEF : {statusData[StatusType.DEF].ToString("N0")}";

        for (int i = 0; i < skillButtons.Length; i++)
        {
            skillButtons[i].Init(i, data.skill_level[i], data.IsSkillLock(i), OpenSkillInfo);
        }

        for (int i = 0; i < equipmentButtons.Length; i++)
        {
            var equipment = i < data.equipments.Count ? data.equipments[i] : null;
            equipmentButtons[i].Init(i, equipment, data.IsEquipmentLock(i), OpenEquipmentInfo);
        }
    }

    void OpenSkillInfo(int index, int level)
    {
        throw new Exception();
    }

    void OpenEquipmentInfo(EquipmentData data, int slot)
    {
        throw new Exception();
    }
}
