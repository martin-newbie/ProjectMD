using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoButton : MonoBehaviour
{
    [SerializeField] Image unitProfileImg;
    [SerializeField] Text unitNameTxt;
    [SerializeField] Text unitLevelTxt;
    [SerializeField] Text unitRankTxt;

    UnitData linkedData;
    Action<UnitData> clickAction;

    public void InitButton(UnitData _linkedData, Action<UnitData> _clickAction = null)
    {
        unitProfileImg.sprite = ResourceManager.GetUnitProfile(_linkedData.index);
        unitNameTxt.text = StaticDataManager.GetConstUnitData(_linkedData.index).name;
        unitLevelTxt.text = "Lv." + _linkedData.level.ToString();
        unitRankTxt.text = _linkedData.rank.ToString();

        linkedData = _linkedData;
        clickAction = _clickAction;
    }

    public void OnbuttonClick()
    {
        clickAction?.Invoke(linkedData);
    }
}
