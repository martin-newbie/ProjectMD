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

    public void Open(UnitData data)
    {
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

    }
}
