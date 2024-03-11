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

    Action<EquipmentData> onButtonAction;
    EquipmentData linkedData;

    public void Init(EquipmentData linkedData, bool locked, Action<EquipmentData> onAction)
    {
        this.linkedData = linkedData;
        onButtonAction = onAction;

        lockedObject.SetActive(locked);
        tierBox.SetActive(!locked);
        tierText.text = "T" + linkedData.tier.ToString();
    }

    public void OnInfoButtonClick()
    {
        onButtonAction?.Invoke(linkedData);
    }
}
