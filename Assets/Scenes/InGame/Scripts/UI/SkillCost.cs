using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCost : MonoBehaviour
{

    public Text costText;
    public CostUnit[] costUnits;
    bool isInit = false;

    void Start()
    {
        for (int i = 0; i < costUnits.Length; i++)
        {
            costUnits[i].InitUnit(i);
        }
        isInit = true;
    }

    public void SetCostValueUI(float value)
    {
        if (!isInit) return;

        foreach (var unit in costUnits)
        {
            unit.SetCostFill(value);
        }
    }

    public void CostRecoverAction(int cost)
    {
        // set position
        // recover effect

        costText.text = Mathf.FloorToInt(cost).ToString();
        // text effect
    }
}
