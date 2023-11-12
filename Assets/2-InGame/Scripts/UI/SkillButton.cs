using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField] Image inImage;

    [HideInInspector] public RectTransform rect;

    // for test or replace as sprite
    Color[] attributeColors = new Color[]
    {
        new Color(0.8f,0,0),
        new Color(0.8f,0.8f,0),
        new Color(0.2f,0.8f,0),
        new Color(0,0.8f,0.8f),
        new Color(0,0.2f,0.8f),
        new Color(0.4f,0f,0.8f),
    };

    SkillCanvas manager;
    public int attribute;

    public void InitButton(SkillCanvas _manager)
    {
        manager = _manager;
        rect = GetComponent<RectTransform>();
    }

    public void SetData(int _attribute)
    {
        attribute = _attribute;
        inImage.color = attributeColors[attribute];
    }

    public void MovePos(Vector3 start, Vector3 target)
    {
        StartCoroutine(moveRoutine());

        IEnumerator moveRoutine()
        {
            float dur = Vector3.Distance(start, target) * 0.0003f;
            float timer = 0f;

            while (timer < dur)
            {
                rect.anchoredPosition = Vector3.Lerp(start, target, timer / dur);
                timer += Time.deltaTime;
                yield return null;
            }

            rect.anchoredPosition = target;
        }
    }

    public void UseSkill()
    {
        // use other collabs skills first
        // transfer buff data to character
        // make character user skill
        // disconnect data from character

        int collabsCount = 0;
        int thisIdx = manager.activatingBtn.IndexOf(this);
        var left = thisIdx - 1;
        var right = thisIdx + 1;
        List<SkillButton> usesBtn = new List<SkillButton>();
        bool leftFinish = false, rightFinish = false;

        while (!leftFinish || !rightFinish)
        {
            if (left >= 0 && collabsCount < 4 && manager.activatingBtn[left].attribute == attribute)
            {
                usesBtn.Add(manager.activatingBtn[left]);
                left = left - 1;
                collabsCount++;
            }
            else
            {
                leftFinish = true;
            }

            if (right < manager.activatingBtn.Count && collabsCount < 4 && manager.activatingBtn[right].attribute == attribute)
            {
                usesBtn.Add(manager.activatingBtn[right]);
                right = right + 1;
                collabsCount++;
            }
            else
            {
                rightFinish = true;
            }
        }

        foreach (var item in usesBtn)
        {
            item.UseSkillAsCollabs();
        }
        manager.PushSkillButton(this);
    }

    public void UseSkillAsCollabs()
    {
        manager.PushSkillButton(this);
    }
}
