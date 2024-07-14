using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostUnit : MonoBehaviour
{
    [SerializeField] Image fillingImg;
    [SerializeField] GameObject filledObj;

    int unitIdx;

    public void InitUnit(int index)
    {
        unitIdx = index;
    }

    public void SetCostFill(float cost)
    {
        float fill = cost - unitIdx;
        if (fill < 0)
        {
            fillingImg.fillAmount = 0;
            filledObj.SetActive(false);
            return;
        }

        if(cost > unitIdx + 1)
        {
            fillingImg.fillAmount = 0;
            filledObj.SetActive(true);
        }
        else
        {
            fillingImg.fillAmount = fill;
            filledObj.SetActive(false);
        }
    }
}
