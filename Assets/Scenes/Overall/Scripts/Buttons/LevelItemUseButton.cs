using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelItemUseButton : MonoBehaviour
{
    [SerializeField] ItemInfo itemInfo;
    [SerializeField] Text selectedCountText;
    [SerializeField] GameObject selectedObject;
    [SerializeField] GameObject reduceButton;

    ItemData item;
    int selectCount;

    public void Init(int itemIndex)
    {
        itemInfo.InitInfo(itemIndex);
        selectedCountText.text = "x0";
        selectedObject.SetActive(false);
        reduceButton.SetActive(false);

        item = UserData.Instance.items.Find(i => i.idx == itemIndex);
        if (item == null)
        {
            // set item icon blur
        }
    }

    public void OnRaiseButton()
    {
        if (item == null || selectCount >= item.count)
        {
            // error message
            return;
        }

        selectCount++;
        selectedObject.SetActive(true);
        reduceButton.SetActive(true);
        selectedCountText.text = "x" + selectCount.ToString();
    }

    public void OnReduceButton()
    {
        if (selectCount <= 0)
        {
            // it can't be
            return;
        }

        selectCount--;
        selectedCountText.text = "x" + selectCount.ToString();
        if (selectCount == 0)
        {
            selectedObject.SetActive(false);
            reduceButton.SetActive(false);
        }
    }
}
