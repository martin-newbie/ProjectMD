using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverallStatusPanel : MonoBehaviour
{

    [SerializeField] Button[] panelButtons;
    [SerializeField] GameObject[] panelObjects;

    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            int idx = i;
            panelButtons[i].onClick.AddListener(() => OpenPanelObject(idx));
            panelObjects[i].SetActive(false);
        }

        OpenPanelObject(0);
    }

    void OpenPanelObject(int idx)
    {
        foreach (var item in panelObjects)
        {
            item.SetActive(false);
        }
        panelObjects[idx].SetActive(true);
    }

    public void InitCharacter(UnitData _linkedData)
    {

    }
}
