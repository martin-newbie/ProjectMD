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

    public void CostRecoverAction(int cost)
    {
        // set position
        // recover effect

        costText.text = Mathf.FloorToInt(cost).ToString();
        // text effect
    }
}
