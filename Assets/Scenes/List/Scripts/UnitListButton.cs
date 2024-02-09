using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitListButton : MonoBehaviour
{
    [SerializeField] Image unitProfileImg;
    [SerializeField] Text unitNameTxt;

    public void InitButton(UnitData _linkedData)
    {
        unitProfileImg.sprite = ResourceManager.GetProfile(_linkedData.index);
        unitNameTxt.text = StaticDataManager.GetConstUnitData(_linkedData.index).name;
    }

    public void OnbuttonClick()
    {

    }
}
