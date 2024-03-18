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
    [SerializeField] Text coinText;

    [SerializeField] Transform inventory;
    [SerializeField] EquipmentItem itemPrefab;
    List<EquipmentItem> equipmentItems;
    bool itemInit;

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
        totalExp = 0;
        calculateExp = 0;
        calculateLevel = 0;

        if (!itemInit)
        {
            itemInit = true;
            equipmentItems = new List<EquipmentItem>();
            var datas = UserData.Instance.FindSpeciesItems(3);
            for (int i = 0; i < datas.Length; i++)
            {
                var item = Instantiate(itemPrefab, inventory);
                item.Init(this, datas[i]);
                equipmentItems.Add(item);
            }
        }

        UpdateInterface();
    }

    public void RaiseExp(int amount)
    {
        totalExp += amount;
        if (totalExp > GetMaximizeExp()) totalExp = GetMaximizeExp();
        calculateExp = totalExp;
        calculateLevel = linkedData.level;

        UpdateInterface();
    }

    public void ReduceExp(int amount)
    {
        totalExp -= amount;
        calculateExp = totalExp;
        calculateLevel = linkedData.level;

        UpdateInterface();
    }

    public void ClearSelectedData()
    {
        foreach (var item in equipmentItems)
        {
            item.ClearSelect();
        }
    }

    public bool NoMoreItem()
    {
        return totalExp >= GetMaximizeExp();
    }

    public int GetMaximizeExp()
    {
        int exp = 0;
        int maxLevel = StaticDataManager.GetEquipmentValueData(linkedData.index, linkedData.tier).max_level - 1;
        int curLevel = linkedData.level;
        while (curLevel <= maxLevel)
        {
            exp += GetLevelExp(maxLevel);
            maxLevel--;
        }
        exp -= linkedData.exp;
        return exp;
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
        updateExpGauge.fillAmount = calculateLevel == valueData.max_level ? 1 : (float)calculateExp / (float)GetLevelExp(calculateLevel);
        levelText.text = "Lv." + (linkedData.level + 1).ToString();

        SetAbilityText(valueData);
        if (levelUpdated)
        {
            expGauge.fillAmount = 0f;
            levelText.text += OverallManager.Instance.ModifyUpgradedColorText(calculateLevel + 1);
        }

        coinText.text = (totalExp * 250).ToString("N0");
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

    public void OnUpgradeButton()
    {
        int requireCoin = totalExp * 250;
        if (totalExp == 0) return;
        if (requireCoin > UserData.Instance.coin) return;

        var sendData = new SendEquipmentUpgrade();
        sendData.uuid = UserData.Instance.uuid;
        sendData.id = linkedData.unit_id;
        sendData.place = linkedData.place_index;
        sendData.use_items = GetRequireItems();
        sendData.use_coin = requireCoin;
        sendData.update_exp = totalExp;

        WebRequest.Post("unit/upgrade-equipment", JsonUtility.ToJson(sendData), (data) =>
        {
            linkedData.UpdateExp(totalExp);
            Open(linkedData);
        });
    }

    ItemData[] GetRequireItems()
    {
        var result = new List<ItemData>();

        foreach (var item in equipmentItems)
        {
            if (item.selectCount > 0)
            {
                var data = new ItemData();
                data.idx = item.linkedData.idx;
                data.count = item.selectCount;
                result.Add(data);
            }
        }

        return result.ToArray();
    }
}

public class SendEquipmentUpgrade
{
    public string uuid;
    public int id;
    public int place;
    public ItemData[] use_items;
    public int use_coin;
    public int update_exp;
}