using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutSelectButton : MonoBehaviour
{

    [HideInInspector] public UnitData LinkedData;
    LoadoutSelectPanel panel;

    [SerializeField] Image profileImage;
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
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

        profileImage.sprite = ResourceManager.GetUnitProfile(LinkedData.index);
        nameText.text = StaticDataManager.GetConstUnitData(LinkedData.index).name;

        if (panel.curSelected.Contains(LinkedData.id))
        {
            selectedCurrent.SetActive(true);
        }
    }

    public void OnSelect()
    {
        panel.SelectButton(LinkedData.id);
    }
}
