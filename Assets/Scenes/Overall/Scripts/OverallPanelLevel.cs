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
        calculatedExp = totalExp + linkedData.exp;
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
            levelText.text = "Lv." + (linkedData.level + 1).ToString() + OverallManager.Instance.ModifyUpgradedColorText(calculatedLevel + 1);

            var statusData = StaticDataManager.GetUnitStatus(linkedData.index);
            var prevStatus = statusData.GetCalculatedValueDictionary(linkedData.level);
            var nextStatus = statusData.GetCalculatedValueDictionary(calculatedLevel);
            atkText.text = OverallManager.Instance.GetStatusText(StatusType.DMG, prevStatus) + OverallManager.Instance.ModifyUpgradedColorText(nextStatus[StatusType.DMG]);
            hpText.text = OverallManager.Instance.GetStatusText(StatusType.DMG, prevStatus) + OverallManager.Instance.ModifyUpgradedColorText(nextStatus[StatusType.HP]);
            defText.text = OverallManager.Instance.GetStatusText(StatusType.DMG, prevStatus) + OverallManager.Instance.ModifyUpgradedColorText(nextStatus[StatusType.DEF]);
        }
    }

    void ClearModifyStatus()
    {
        totalExp = 0;
        calculatedExp = 0;
        calculatedLevel = 0;

        curExpGauge.fillAmount = (float)linkedData.exp / (float)GetLevelExp(linkedData.level);
        newExpGauge.fillAmount = 0;
        levelText.text = "Lv." + (linkedData.level + 1).ToString();
        var statusData = StaticDataManager.GetUnitStatus(linkedData.index).GetCalculatedValueDictionary(linkedData.level);
        atkText.text = OverallManager.Instance.GetStatusText(StatusType.DMG, statusData);
        hpText.text = OverallManager.Instance.GetStatusText(StatusType.HP, statusData);
        defText.text = OverallManager.Instance.GetStatusText(StatusType.DEF, statusData);
        useCoinText.text = "0";
    }

    public void OnUpgradeButton()
    {
        int useCoin = totalExp;
        if (UserData.Instance.coin < useCoin) return;

        var sendData = new SendLevelupData();
        sendData.uuid = UserData.Instance.uuid;
        sendData.id = linkedData.id;
        sendData.use_items = GetUseLevelItems();
        sendData.use_coin = useCoin;
        sendData.updated_exp = totalExp;

        WebRequest.Post("unit/upgrade-level", JsonUtility.ToJson(sendData), (data) =>
        {
            linkedData.UpdateExp(sendData.updated_exp);
            UserData.Instance.coin -= sendData.use_coin;
            UserData.Instance.UseManyItem(sendData.use_items);
            Open(linkedData);
        });
    }

    ItemData[] GetUseLevelItems()
    {
        List<ItemData> datas = new List<ItemData>();
        for (int i = 0; i < levelItemButtons.Length; i++)
        {
            if(levelItemButtons[i].selectCount > 0)
            {
                ItemData data = new ItemData();
                data.idx = levelItemButtons[i].item.idx;
                data.count = levelItemButtons[i].selectCount;
                datas.Add(data);
            }
        }
        return datas.ToArray();
    }
}

public class SendLevelupData
{
    public string uuid;
    public int id;
    public ItemData[] use_items;
    public int use_coin;
    public int updated_exp;
}