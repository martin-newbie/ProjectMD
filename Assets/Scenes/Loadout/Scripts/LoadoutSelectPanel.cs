using System.Collections.Generic;
using UnityEngine;

public class LoadoutSelectPanel : MonoBehaviour
{

    [SerializeField] LoadoutSelectButton buttonPrefab;
    [SerializeField] Transform buttonLayout;
    List<LoadoutSelectButton> buttons = new List<LoadoutSelectButton>();
    [HideInInspector] public List<int> curSelected;

    int deckIdx;

    public void InitPanel()
    {
        var list = UserData.Instance.unitDatas;

        for (int i = 0; i < list.Count; i++)
        {
            var temp = Instantiate(buttonPrefab, buttonLayout);
            temp.InitButton(this);
            buttons.Add(temp);
        }
    }

    public void OpenPanel(int[] selected, int deck)
    {
        gameObject.SetActive(true);
        curSelected = new List<int>(selected);
        deckIdx = deck;

        var unitDatas = SortUnitData(UserData.Instance.unitDatas);
        SetButtonsData(unitDatas);
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

        SetButtonsData(UserData.Instance.unitDatas);
    }

    void SetButtonsData(List<UnitData> unitDatas)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            var btn = buttons[i];
            btn.SetButtonData(unitDatas[i]);
        }
    }

    List<UnitData> SortUnitData(List<UnitData> unitDatas)
    {
        for (int i = 0; i < unitDatas.Count; i++)
        {
            var item = unitDatas[i];

            if (curSelected.Contains(item.unitIdx))
            {
                unitDatas.Remove(item);
                unitDatas.Insert(0, item);
            }
            else if (UserData.Instance.AlreadySelected(item.unitIdx))
            {
                unitDatas.Remove(item);
                unitDatas.Add(item);
            }
        }

        return unitDatas;
    }

    public void OnConfirmButton()
    {
        UserData.Instance.SetDeckUnitAt(curSelected.ToArray(), deckIdx);
        LoadoutManager.Instance.UpdateDeck(deckIdx);
        gameObject.SetActive(false);
    }
}
