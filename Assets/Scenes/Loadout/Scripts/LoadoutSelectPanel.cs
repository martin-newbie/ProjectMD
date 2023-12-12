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
        var list = UserData.Instance.unitDatas;
        foreach (var item in list)
        {
            var temp = Instantiate(buttonPrefab, buttonLayout);
            temp.InitButton(item, this);
            buttons.Add(temp);
        }
    }

    public void OpenPanel(int[] selected)
    {
        gameObject.SetActive(true);
        curSelected = new List<int>(selected);

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
        InitButtonStateUI();
    }

    void InitButtonStateUI()
    {
        // Todo later
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].SetSelectOutline(curSelected.Contains(buttons[i].LinkedData.unitIdx));
        }
    }

    public void OnConfirmButton()
    {
        LoadoutManager.Instance.UpdateDeck(curSelected.ToArray());
        gameObject.SetActive(false);
    }
}
