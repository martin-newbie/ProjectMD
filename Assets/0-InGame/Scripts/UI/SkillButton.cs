using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField] Image inImage;

    [HideInInspector] public RectTransform rect;
    [HideInInspector] public SkillButton leftBtn;
    [HideInInspector] public SkillButton rightBtn;

    // for test or replace as sprite
    Color[] attributeColors = new Color[]
    {
        new Color(0.8f,0,0),
        new Color(0.8f,0.8f,0),
        new Color(0.2f,0.8f,0),
        new Color(0,0.8f,0.8f),
        new Color(0,0.8f,0.8f),
        new Color(0,0.2f,0.8f),
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

        List<SkillButton> collabsSkills = new List<SkillButton>();
        var searchLeft = this;
        var searchRight = this;
        int searchCount = 0;
        while (searchLeft.leftBtn != null && searchCount < 5)
        {
            if (searchLeft.attribute != searchLeft.leftBtn.attribute) break;

            collabsSkills.Add(searchLeft.leftBtn);
            searchLeft = searchLeft.leftBtn;
            searchCount++;
        }
        while (searchRight.rightBtn != null && searchCount < 5)
        {
            if (searchRight.attribute != searchRight.rightBtn.attribute) break;

            collabsSkills.Add(searchRight.rightBtn);
            searchRight = searchRight.rightBtn;
            searchCount++;
        }

        foreach (var item in collabsSkills)
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
