using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverallPanelSkill : MonoBehaviour, OverallPanel
{

    public SkillInfoButton[] skillButtons;
    public Text[] skillDescTexts;

    public void Open(UnitData data)
    {
        for (int i = 0; i < skillButtons.Length; i++)
        {
            skillButtons[i].Init(i, data.skill_level[i], data.IsSkillUnlock(i), OpenSkillUpgrade);
        }

        for (int i = 0; i < skillDescTexts.Length; i++)
        {
            // get localization skill desc string
            string localizeSkillDesc = "";
            skillDescTexts[i].text = localizeSkillDesc;
        }
    }

    void OpenSkillUpgrade(int index, int level)
    {
        throw new System.Exception();
    }
}
