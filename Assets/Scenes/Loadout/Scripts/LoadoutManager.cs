using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutManager : MonoBehaviour
{
    [SerializeField] CharacterInfoUI[] infoButtons;

    private void Start()
    {
        foreach (var item in infoButtons)
        {
            item.InitInfo(null);
        }
    }


}
