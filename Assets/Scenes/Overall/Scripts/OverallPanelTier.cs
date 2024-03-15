using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverallPanelTier : MonoBehaviour, IOverallPanel
{

    [SerializeField] GameObject[] tierStars;
    [SerializeField] Text atkText;
    [SerializeField] Text hpText;
    [SerializeField] Text defText;
    [SerializeField] Text unlockText;
    [SerializeField] Text requireItemText;
    [SerializeField] Text requireCoinText;

    UnitData linkedData;

    public void Open(UnitData data)
    {
        linkedData = data;

        var prevStatus = StaticDataManager.GetUnitStatus(data.index).GetCalculatedValueDictionary(0, data.rank);
        atkText.text = OverallManager.Instance.GetStatusText(StatusType.DMG, prevStatus);
        hpText.text = OverallManager.Instance.GetStatusText(StatusType.HP, prevStatus);
        defText.text = OverallManager.Instance.GetStatusText(StatusType.DEF, prevStatus);
        OpenTierStar(data);

        if (data.rank < 4)
        {
            var nextStatus = StaticDataManager.GetUnitStatus(data.index).GetCalculatedValueDictionary(0, data.rank + 1);
            atkText.text += OverallManager.Instance.ModifyUpgradedColorText(nextStatus[StatusType.DMG]);
            hpText.text += OverallManager.Instance.ModifyUpgradedColorText(nextStatus[StatusType.HP]);
            defText.text += OverallManager.Instance.ModifyUpgradedColorText(nextStatus[StatusType.DEF]);
            // init unlock text
        }

        var requireItem = DataManager.GetTierItem(data.rank);
        requireCoinText.text = requireItem.coin_require.ToString("N0");
        requireItemText.text = $"{requireItem.item_require} / {UserData.Instance.FindItem(88).count}";
        // init require item and coin text with data manager
    }

    void OpenTierStar(UnitData data)
    {
        for (int i = 0; i < tierStars.Length; i++)
        {
            tierStars[i].SetActive(i <= data.rank);
        }
    }

    public void Upgrade()
    {
        if (linkedData.rank >= 4) return;
        var requireItem = DataManager.GetTierItem(linkedData.rank);
        if (requireItem.item_require > UserData.Instance.FindItem(88).count) return;
        if (requireItem.coin_require > UserData.Instance.coin) return;

        var sendData = new SendRankLevelup();
        sendData.uuid = UserData.Instance.uuid;
        sendData.id = linkedData.id;
        sendData.use_items = new ItemData[1] { new ItemData() { idx = 88, count = requireItem.item_require } };
        sendData.coin = requireItem.coin_require;

        WebRequest.Post("unit/upgrade-rank", JsonUtility.ToJson(sendData), (data) =>
        {
            linkedData.rank++;
            UserData.Instance.UseItem(sendData.use_items[0]);
            UserData.Instance.coin -= sendData.coin;
            Open(linkedData);
        });
    }
}

[System.Serializable]
public class SendRankLevelup
{
    public string uuid;
    public int id;
    public ItemData[] use_items;
    public int coin;
}