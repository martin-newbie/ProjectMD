using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverallStatusPanel : MonoBehaviour
{

    [SerializeField] Button[] panelButtons;
    [SerializeField] GameObject[] panelObjects;
    UnitData data;

    private void Start()
    {
        for (int i = 0; i < panelButtons.Length; i++)
        {
            int idx = i;
            panelButtons[i].onClick.AddListener(() => OpenPanelObject(idx));
            panelObjects[i].SetActive(false);
        }

    }

    public void InitCharacter(UnitData data)
    {
        this.data = data;
        OpenPanelObject(0);
    }

    void OpenPanelObject(int idx)
    {
        foreach (var item in panelObjects)
        {
            item.SetActive(false);
        }
        panelObjects[idx].SetActive(true);
        panelObjects[idx].GetComponent<IOverallPanel>()?.Open(data);
    }
}


public interface IOverallPanel
{
    void Open(UnitData data);
}