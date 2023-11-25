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
    List<ActiveSkillBehaviour> curActiveUnits = new List<ActiveSkillBehaviour>();
    List<ActiveSkillBehaviour> deckSkills = new List<ActiveSkillBehaviour>();

    float skillDelay = 1f;
    float curDelay;

    public void InitSkills(ActiveSkillBehaviour[] activeUnits)
    {
        curActiveUnits = new List<ActiveSkillBehaviour>(activeUnits);
    }

    public void StartGame()
    {
        active = true;
        deckSkills.Clear();
    }

    private void Update()
    {
        if (!active) return;

        if (curDelay >= skillDelay)
        {
            curDelay = 0;
            AddSkillInDeck();
        }
        curDelay += Time.deltaTime;
    }

    public void RemoveCharacterAtSkills(ActiveSkillBehaviour retiredUnit)
    {
        curActiveUnits.Remove(retiredUnit);
        for (int i = 0; i < deckSkills.Count; i++)
        {
            if (deckSkills[i] == retiredUnit)
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
        List<ActiveSkillBehaviour> collabse = new List<ActiveSkillBehaviour>();
        bool leftFinish = false, rightFinish = false;
        int startIdx = idx;

        while (!leftFinish || !rightFinish)
        {
            if (left >= 0 && collabseCount < 4 && deckSkills[left] == deckSkills[idx])
            {
                collabse.Add(deckSkills[left]);
                startIdx = left;
                left = left - 1;
                collabseCount++;
            }
            else
            {
                leftFinish = true;
            }

            if (right < deckSkills.Count && collabseCount < 4 && deckSkills[right] == deckSkills[idx])
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


        // remove collabse skills
        if (collabseCount > 0)
        {
            deckSkills.RemoveRange(startIdx, collabseCount + 1);
            skillCanvas.RemoveButtonRange(startIdx, collabseCount + 1);
        }
        else
        {
            deckSkills.RemoveAt(idx);
            skillCanvas.RemoveButtonAt(idx);
        }

    }
}
