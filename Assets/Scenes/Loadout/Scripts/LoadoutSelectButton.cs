using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutSelectButton : MonoBehaviour
{

    [HideInInspector] public UnitData LinkedData;
    LoadoutSelectPanel panel;

    [SerializeField] UnitInfoButton info;
    [SerializeField] GameObject selectedCurrent;

    public void InitButton(LoadoutSelectPanel _panel)
    {
        panel = _panel;
        selectedCurrent.SetActive(false);
    }

    public void SetButtonData(UnitData _linkedData)
    {
        LinkedData = _linkedData;
        selectedCurrent.SetActive(false);

        info.InitButton(LinkedData, OnSelect);

        if (panel.curSelected.Contains(LinkedData.id))
        {
            selectedCurrent.SetActive(true);
        }
    }

    public void OnSelect(UnitData linkedData)
    {
        panel.SelectButton(LinkedData.id);
    }
}
