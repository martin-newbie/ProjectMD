using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoButton : MonoBehaviour
{

    [SerializeField] private Image icon;
    [SerializeField] private Text levelText;
    [SerializeField] private GameObject lockedObject;

    Action<int,int> onButtonClickAction;
    int skillLevel;
    int skillIndex;

    public void Init(int index, int level, bool locked, Action<int,int> onClick)
    {
        skillLevel = level;
        skillIndex = index;
        onButtonClickAction = onClick;
        lockedObject.SetActive(locked);
        levelText.gameObject.SetActive(!locked);

        if ((index == 0 && level == 5) || level == 10)
        {
            levelText.text = "max";
        }
        else
        {
            levelText.text = "Lv." + level.ToString();
        }
    }

    public void OnButtonClick()
    {
        onButtonClickAction?.Invoke(skillIndex, skillLevel);
    }
}