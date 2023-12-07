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
    [SerializeField] GameObject selectedOutline;

    public void InitButton(UnitData _linkedData, LoadoutSelectPanel _panel)
    {
        LinkedData = _linkedData;
        panel = _panel;

        profileImage.sprite = ResourceManager.GetProfile(LinkedData.unitIdx);
        nameText.text = StaticDataManager.GetConstUnitData(LinkedData.unitIdx).name;

        selectedOutline.SetActive(false);
    }

    public void SetSelectOutline(bool active)
    {
        selectedOutline.SetActive(active);
    }

    public void OnSelect()
    {
        panel.SelectButton(LinkedData.unitIdx);
    }
}
