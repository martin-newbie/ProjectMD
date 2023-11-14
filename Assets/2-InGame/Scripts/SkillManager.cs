using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    bool active = false;
    ActiveAbleBehaviour[] curActiveUnits;
    List<ActiveAbleBehaviour> deckSkills = new List<ActiveAbleBehaviour>();

    public void InitSkills(ActiveAbleBehaviour[] activeUnits)
    {
        curActiveUnits = activeUnits;
    }

    public void StartGame()
    {
        active = true;
        deckSkills.Clear();
    }

    private void Update()
    {
        if (!active) return;


    }
}
