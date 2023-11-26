using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LoadoutSelectPanel : MonoBehaviour
{

    [SerializeField] LoadoutSelectButton buttonPrefab;
    [SerializeField] Transform buttonLayout;
    List<LoadoutSelectButton> buttons = new List<LoadoutSelectButton>();

    List<int> curSelected;

    public void InitPanel()
    {
        var list = UserData.Instance.charDatas;
        foreach (var item in list)
        {
            var temp = Instantiate(buttonPrefab, buttonLayout);
            temp.InitButton(item, this);
        }
    }

    public void OpenPanel(int[] selected)
    {
        gameObject.SetActive(true);
        curSelected = selected.ToList();

        InitButtonStateUI();
    }

    public void SelectButton(int charIdx)
    {
        if (curSelected.Contains(charIdx))
        {
            curSelected.Remove(charIdx);
        }
        else
        {
            curSelected.Add(charIdx);
        }
    }

    void InitButtonStateUI()
    {
        // Todo later
    }

    public void OnConfirmButton()
    {
        LoadoutManager.Instance.UpdateDeck(curSelected.ToArray());
        gameObject.SetActive(false);
    }
}
