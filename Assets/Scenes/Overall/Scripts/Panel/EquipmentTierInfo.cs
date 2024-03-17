using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentTierInfo : MonoBehaviour
{
    [SerializeField] Image iconImage;
    [SerializeField] Text tierText;
    [SerializeField] Text nameText;
    [SerializeField] Image expGauge;
    [SerializeField] Text levelText;
    [SerializeField] Text descText;

    public void InitInfo(EquipmentValueData data, int level, float expFill = 0f)
    {
        tierText.text = "T" + data.tier.ToString();
        levelText.text = "Lv." + level.ToString();
        expGauge.fillAmount = expFill;
    }
}
