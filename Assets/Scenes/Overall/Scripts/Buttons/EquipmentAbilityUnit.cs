using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentAbilityUnit : MonoBehaviour
{
    [SerializeField] Text text;

    public void Init(string desc)
    {
        text.text = desc;
    }
}
