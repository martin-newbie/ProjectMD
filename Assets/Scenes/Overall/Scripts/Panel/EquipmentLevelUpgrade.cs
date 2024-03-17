using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentLevelUpgrade : MonoBehaviour
{

    [SerializeField] Image iconImage;
    [SerializeField] Text tierText;
    [SerializeField] Text titleText;
    [SerializeField] Text levelText;
    [SerializeField] Image updateExpGauge;
    [SerializeField] Image expGauge;
    [SerializeField] EquipmentAbilityUnit[] abilities;

    EquipmentData linkedData;
    int totalExp;
    int calculateExp;
    int calculateLevel;

    public void Open(EquipmentData data)
    {
        gameObject.SetActive(true);
        linkedData = data;

        tierText.text = "T" + data.tier.ToString();
        updateExpGauge.fillAmount = 0f;
        ClearExp();
    }

    public void ClearExp()
    {
        totalExp = 0;
        calculateExp = 0;
        calculateLevel = linkedData.level;

        UpdateInterface();
    }

    public void UpdateExp(int amount)
    {
        totalExp += amount;
        calculateExp = totalExp;
        calculateLevel = linkedData.level;

        UpdateInterface();
    }

    void UpdateInterface()
    {
        bool levelUpdated = calculateExp >= GetLevelExp(calculateLevel);
        while (calculateExp >= GetLevelExp(calculateLevel))
        {
            calculateExp -= GetLevelExp(calculateLevel);
            calculateLevel++;
        }

        var valueData = StaticDataManager.GetEquipmentValueData(linkedData.index, linkedData.tier);

        expGauge.fillAmount = (float)linkedData.exp / (float)DataManager.GetEquipmentLevelExp(linkedData.level);
        updateExpGauge.fillAmount = (float)calculateExp / (float)GetLevelExp(calculateLevel);
        levelText.text = "Lv." + linkedData.level.ToString();

        SetAbilityText(valueData);
        if (levelUpdated)
        {
            expGauge.fillAmount = 0f;
            levelText.text += OverallManager.Instance.ModifyUpgradedColorText(calculateLevel + 1);
        }
    }

    void SetAbilityText(EquipmentValueData valueData)
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            var ability = abilities[i];
            if (i < valueData.buff_type.Length)
            {
                ability.gameObject.SetActive(true);
                ability.Init(GetAbilityText(valueData.buff_type[i], valueData.GetLevelBuff(linkedData.level, i), linkedData.level == calculateLevel ? 0f : valueData.GetLevelBuff(calculateLevel, i)));
            }
            else
            {
                ability.gameObject.SetActive(false);
            }
        }
    }

    string GetAbilityText(int statusIdx, float prevValue, float nextValue = 0)
    {
        string upgraded = nextValue != 0 ? OverallManager.Instance.ModifyUpgradedColorText(nextValue) : "";
        return ((StatusType)statusIdx).ToString() + " : " + prevValue.ToString("N0") + upgraded;
    }

    int GetLevelExp(int level)
    {
        return DataManager.GetEquipmentLevelExp(level);
    }
}
