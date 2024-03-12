using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverallPanelLevel : MonoBehaviour, IOverallPanel
{

    // running values
    int totalExp;
    int calculatedExp;
    int calculatedLevel;
    UnitData linkedData;

    [Header("Exp Area")]
    [SerializeField] Text levelText;
    [SerializeField] Image curExpGauge;
    [SerializeField] Image newExpGauge;

    // these status are not affect in equipment, rank, and any other unit data
    // those status are only calculated by static level data
    [Header("Status Area")]
    [SerializeField] Text atkText;
    [SerializeField] Text hpText;
    [SerializeField] Text defText;

    [Header("Item Area")]
    [SerializeField] LevelItemUseButton[] levelItemButtons;

    [Header("Upgrade Area")]
    [SerializeField] Text useCoinText;
    [SerializeField] Text upgradeButtonText; // it needed to modify text with localized text

    public void Open(UnitData data)
    {
        linkedData = data;
        for (int i = 0; i < levelItemButtons.Length; i++)
        {
            levelItemButtons[i].Init(i, this);
        }
        ClearModifyStatus();
    }

    public void RaiseExp(int exp)
    {
        totalExp += exp;

        UpdateModifyStatus();
    }

    int GetLevelExp(int level)
    {
        return ResourceManager.GetUnitLevelupExp(level);
    }

    public void ReduceExp(int exp)
    {
        totalExp -= exp;

        if (totalExp <= 0)
        {
            ClearModifyStatus();
        }
        else
        {
            UpdateModifyStatus();
        }
    }

    void UpdateModifyStatus()
    {
        calculatedExp = totalExp;
        calculatedLevel = linkedData.level;
        while (calculatedExp >= GetLevelExp(calculatedLevel))
        {
            calculatedExp -= GetLevelExp(calculatedLevel);
            calculatedLevel++;
        }

        curExpGauge.fillAmount = calculatedLevel > linkedData.level ? 0 : (float)linkedData.exp / (float)GetLevelExp(linkedData.level);
        newExpGauge.fillAmount = (float)calculatedExp / (float)GetLevelExp(calculatedLevel);
        useCoinText.text = totalExp.ToString("N0");

        if (linkedData.level < calculatedLevel)
        {
            levelText.text = "Lv." + (linkedData.level + 1).ToString() + ModifyUpgradedColorText($" → {calculatedLevel + 1}");

            var statusData = StaticDataManager.GetUnitStatus(linkedData.index);
            var prevStatus = statusData.GetCalculatedValueDictionary(linkedData.level);
            var nextStatus = statusData.GetCalculatedValueDictionary(calculatedLevel);
            atkText.text = $"ATTACK : {prevStatus[StatusType.DMG].ToString("N0")} {ModifyUpgradedColorText($"→ {nextStatus[StatusType.DMG].ToString("N0")}")}";
            hpText.text = $"HP : {prevStatus[StatusType.HP].ToString("N0")} {ModifyUpgradedColorText($"→ {nextStatus[StatusType.HP].ToString("N0")}")}";
            defText.text = $"DEF : {prevStatus[StatusType.DEF].ToString("N0")} {ModifyUpgradedColorText($"→ {nextStatus[StatusType.DEF].ToString("N0")}")}";
        }
    }

    void ClearModifyStatus()
    {
        calculatedExp = 0;
        calculatedLevel = 0;

        curExpGauge.fillAmount = (float)linkedData.exp / (float)GetLevelExp(linkedData.level);
        newExpGauge.fillAmount = 0;
        levelText.text = "Lv." + (linkedData.level + 1).ToString();
        var statusData = StaticDataManager.GetUnitStatus(linkedData.index).GetCalculatedValueDictionary(linkedData.level);
        atkText.text = $"ATTACK : {statusData[StatusType.DMG].ToString("N0")}";
        hpText.text = $"HP : {statusData[StatusType.HP].ToString("N0")}";
        defText.text = $"DEF : {statusData[StatusType.DEF].ToString("N0")}";
        useCoinText.text = "0";
    }

    // use when upgrade item applied and level modified
    string ModifyUpgradedColorText(string text)
    {
        return "<color=#47E76D>" + text + "</color>";
    }
}
