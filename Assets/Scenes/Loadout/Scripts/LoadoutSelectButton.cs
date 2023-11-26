using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutSelectButton : MonoBehaviour
{

    [HideInInspector] public CharacterData LinkedData;
    LoadoutSelectPanel panel;

    [SerializeField] Image profileImage;
    [SerializeField] Text nameText;

    public void InitButton(CharacterData _linkedData, LoadoutSelectPanel _panel)
    {
        LinkedData = _linkedData;
        panel = _panel;

        profileImage.sprite = ResourceManager.GetProfile(LinkedData.charIdx);
    }

    public void OnSelect()
    {
        panel.SelectButton(LinkedData.charIdx);
    }
}
