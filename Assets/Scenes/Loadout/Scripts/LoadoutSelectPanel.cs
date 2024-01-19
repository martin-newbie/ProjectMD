using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LoadoutSelectPanel : MonoBehaviour
{

    [SerializeField] LoadoutSelectButton buttonPrefab;
    [SerializeField] Transform buttonLayout;
    List<LoadoutSelectButton> buttons = new List<LoadoutSelectButton>();
    [HideInInspector] public List<int> curSelected;

    int deckIdx;
    List<UnitData> marshaledDataList = new List<UnitData>();

    public void InitPanel()
    {
        var list = UserData.Instance.units;

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

        marshaledDataList = SortUnitData(UserData.Instance.units.ToList());
        SetButtonsData();
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

        SetButtonsData();
    }

    void SetButtonsData()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            var btn = buttons[i];
            btn.SetButtonData(marshaledDataList[i]);
        }
    }

    List<UnitData> SortUnitData(List<UnitData> unitDatas)
    {
        var listOrigin = unitDatas.ToList();
        for (int i = 0; i < listOrigin.Count; i++)
        {
            var item = listOrigin[i];

            if (curSelected.Contains(item.index))
            {
                unitDatas.Remove(item);
                unitDatas.Insert(0, item);
            }
            else if (UserData.Instance.AlreadySelected(item.index))
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
