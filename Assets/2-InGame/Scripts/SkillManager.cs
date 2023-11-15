using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance = null;
    private void Awake()
    {
        Instance = this;
    }

    public SkillCanvas skillCanvas;

    bool active = false;
    List<ActiveAbleBehaviour> curActiveUnits = new List<ActiveAbleBehaviour>();
    List<ActiveAbleBehaviour> deckSkills = new List<ActiveAbleBehaviour>();

    float skillDelay = 1f;
    float curDelay;

    public void InitSkills(ActiveAbleBehaviour[] activeUnits)
    {
        curActiveUnits = new List<ActiveAbleBehaviour>(activeUnits);
    }

    public void StartGame()
    {
        active = true;
        deckSkills.Clear();
    }

    private void Update()
    {
        if (!active) return;

        if(curDelay >= skillDelay)
        {
            curDelay = 0;
            AddSkillInDeck();
        }
        curDelay += Time.deltaTime;
    }

    void RemoveCharacterAtSkills(ActiveAbleBehaviour retiredUnit)
    {
        curActiveUnits.Remove(retiredUnit);
        for (int i = 0; i < deckSkills.Count; i++)
        {
            if(deckSkills[i] == retiredUnit)
            {
                deckSkills.RemoveAt(i);
                skillCanvas.RemoveButtonAt(i);
                // skill button remove
                i--;
            }
        }
    }

    void AddSkillInDeck()
    {
        if (deckSkills.Count >= 10) return;

        int rand = Random.Range(0, curActiveUnits.Count);
        var skillData = curActiveUnits[rand];

        deckSkills.Add(skillData);
        skillCanvas.AddSkillButton(skillData);
    }

    public void UseSkill(int idx)
    {
        int collabseCount = 0;
        int left = idx - 1;
        int right = idx + 1;
        List<ActiveAbleBehaviour> collabse = new List<ActiveAbleBehaviour>();
        bool leftFinish = false, rightFinish = false;

        while (!leftFinish || !rightFinish)
        {
            if(left >= 0 && collabseCount < 4 && deckSkills[left].skillType == deckSkills[idx].skillType)
            {
                collabse.Add(deckSkills[left]);
                left = left - 1;
                collabseCount++;
            }
            else
            {
                leftFinish = true;
            }

            if(right < deckSkills.Count && collabseCount < 4 && deckSkills[right].skillType == deckSkills[idx].skillType)
            {
                collabse.Add(deckSkills[right]);
                right = right + 1;
                collabseCount++;
            }
            else
            {
                rightFinish = true;
            }
        }

        var skillValue = deckSkills[idx].GetDefaultSkillValue();
        foreach (var skill in collabse)
        {
            skill.CollabseSkill(skillValue, deckSkills[idx]);
        }
        deckSkills[idx].UseActiveSkill(skillValue);

        if(collabseCount > 0)
        {
            for (int i = left; i <= left + right - idx; i++)
            {
                deckSkills.RemoveAt(left);
                skillCanvas.RemoveButtonAt(left);
                // skill button remove
            }
        }


    }
}
