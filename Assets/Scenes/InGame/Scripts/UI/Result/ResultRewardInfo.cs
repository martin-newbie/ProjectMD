using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultRewardInfo : MonoBehaviour
{
    public Image itemIcon;
    public Text countText;

    public GameObject reasonBox;
    public Text reasonText;

    public void InitRewardInfo(Reward rewardData)
    {
        countText.text = rewardData.count.ToString();

        // TODO : send reward data with reason and apply it
        reasonBox.SetActive(false);

        switch (rewardData.type)
        {
            case 0:
                itemIcon.sprite = ResourceManager.GetItemIcon(rewardData.index);
                break;
            case 1:
                itemIcon.sprite = ResourceManager.GetCurrencyIcon(rewardData.index);
                break;
            case 2:
                itemIcon.sprite = ResourceManager.GetUnitProfile(rewardData.index);
                break;
        }
    }
}
