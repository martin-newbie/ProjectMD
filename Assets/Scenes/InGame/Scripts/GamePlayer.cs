using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayer
{
    public bool isGameActive = false;

    public List<UnitBehaviour> units = new List<UnitBehaviour>();

    // about skill
    List<ActiveSkillBehaviour> skillUnits = new List<ActiveSkillBehaviour>();
    List<ActiveSkillBehaviour> skillDeck = new List<ActiveSkillBehaviour>();

    public float skillCost;
    public float costCharge;

    float skillDelay = 1f;
    float curDelay;

    public GamePlayer(int[] unitIdx, int[] posIdx)
    {
        // spawn and init units
        // set skill delay due to the units
        // set cost charging time due to the units
    }

    public void Update()
    {
        if (!isGameActive) return;

        if (curDelay >= skillDelay)
        {
            curDelay = 0;
            AddSkillInDeck();
        }

        skillCost += Time.deltaTime * costCharge;
        curDelay += Time.deltaTime;
    }


    public void RemoveCharacter(UnitBehaviour retiredUnit)
    {
        units.Remove(retiredUnit);
        InGameManager.Instance.allUnits.Remove(retiredUnit);

        if (retiredUnit is ActiveSkillBehaviour)
        {
            skillUnits.Remove(retiredUnit as ActiveSkillBehaviour);
            for (int i = 0; i < skillDeck.Count; i++)
            {
                if (skillDeck[i] == retiredUnit)
                {
                    skillDeck.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    void AddSkillInDeck()
    {
        if (skillDeck.Count >= 10) return;

        int rand = Random.Range(0, skillUnits.Count);
        var skillData = skillUnits[rand];

        skillDeck.Add(skillData);
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
            if (left >= 0 && collabseCount < 4 && skillDeck[left] == skillDeck[idx])
            {
                collabse.Add(skillDeck[left]);
                startIdx = left;
                left = left - 1;
                collabseCount++;
            }
            else
            {
                leftFinish = true;
            }

            if (right < skillDeck.Count && collabseCount < 4 && skillDeck[right] == skillDeck[idx])
            {
                collabse.Add(skillDeck[right]);
                right = right + 1;
                collabseCount++;
            }
            else
            {
                rightFinish = true;
            }
        }

        var skillValue = skillDeck[idx].GetDefaultSkillValue();
        foreach (var skill in collabse)
        {
            skill.CollabseSkill(skillValue, skillDeck[idx]);
        }
        skillDeck[idx].UseActiveSkill(skillValue);


        // remove collabse skills
        if (collabseCount > 0)
        {
            skillDeck.RemoveRange(startIdx, collabseCount + 1);
        }
        else
        {
            skillDeck.RemoveAt(idx);
        }

    }
}
