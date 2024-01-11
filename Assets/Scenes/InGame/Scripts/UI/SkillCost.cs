using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCost : MonoBehaviour
{
    public Image costImage;
    public Text costText;
    public RectTransform costEffect;

    public void SetCostValueUI(float value)
    {
        costImage.fillAmount = value;
    }

    public void CostRecoverAction(float value)
    {
        // set position
        // recover effect

        costText.text = Mathf.FloorToInt(value / 10).ToString();
        // text effect
    }
}
