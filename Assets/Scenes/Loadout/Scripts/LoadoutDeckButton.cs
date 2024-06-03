using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutDeckButton : MonoBehaviour
{
    [SerializeField] Sprite notSelectedSpr;
    [SerializeField] Sprite selectedSpr;
    [SerializeField] GameObject selectedOutline;
    [SerializeField] Image bodyImg;
    [SerializeField] Text deckNameTxt;

    public void UpdateSelected(bool selected)
    {
        bodyImg.sprite = selected ? selectedSpr : notSelectedSpr;
        bodyImg.SetNativeSize();
        selectedOutline.SetActive(selected);
    }

    public void UpdateTitle(string deckName)
    {
        deckNameTxt.text = deckName;
    }

    public void InitAction(Action onClick)
    {
        GetComponent<Button>().onClick.AddListener(() => onClick?.Invoke());
    }
}
