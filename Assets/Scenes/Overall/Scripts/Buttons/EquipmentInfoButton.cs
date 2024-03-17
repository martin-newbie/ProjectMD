using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentInfoButton : MonoBehaviour
{
    [SerializeField] Image EquipmentIcon;
    [SerializeField] GameObject tierBox;
    [SerializeField] Text tierText;
    [SerializeField] GameObject lockedObject;

    Action<EquipmentData, int> onButtonAction;
    EquipmentData linkedData;
    int slotIndex;

    public void Init(int slot,EquipmentData data, bool locked, Action<EquipmentData, int> onAction)
    {
        linkedData = data;
        slotIndex = slot;
        onButtonAction = onAction;

        lockedObject.SetActive(locked);
        tierBox.SetActive(!locked && data != null);

        if (data != null)
            tierText.text = "T" + data.tier.ToString();
    }

    public void OnInfoButtonClick()
    {
        onButtonAction?.Invoke(linkedData, slotIndex);
    }
}
