using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverallPanelSummary : MonoBehaviour, OverallPanel
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


    int[] skillUnlockRank = new int[4] { 0, 0, 1, 2 };
    int[] equipmentUnlockLevel = new int[3] { 0, 5, 10 };

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
            bool locked = skillUnlockRank[i] > data.rank;
            skillButtons[i].Init(i, data.skill_level[i], locked, OpenSkillInfo);
        }

        for (int i = 0; i < equipmentButtons.Length; i++)
        {
            bool locked = equipmentUnlockLevel[i] > data.level;
            equipmentButtons[i].Init(data.equipments[i], locked, OpenEquipmentInfo);
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
