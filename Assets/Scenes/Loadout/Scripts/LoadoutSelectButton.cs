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
    [SerializeField] GameObject selectedOther;

    public void InitButton(LoadoutSelectPanel _panel)
    {
        panel = _panel;
        selectedCurrent.SetActive(false);
        selectedOther.SetActive(false);
    }

    public void SetButtonData(UnitData _linkedData)
    {
        LinkedData = _linkedData;
        selectedCurrent.SetActive(false);
        selectedOther.SetActive(false);

        profileImage.sprite = ResourceManager.GetProfile(LinkedData.index);
        nameText.text = StaticDataManager.GetConstUnitData(LinkedData.index).name;

        if (panel.curSelected.Contains(LinkedData.index))
        {
            selectedCurrent.SetActive(true);
        }
        else if (UserData.Instance.AlreadySelected(LinkedData.index))
        {
            selectedOther.SetActive(true);
        }
    }

    public void OnSelect()
    {
        panel.SelectButton(LinkedData.index);
    }
}
