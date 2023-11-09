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
        var left = leftBtn;
        var right = rightBtn;
        List<SkillButton> usesBtn = new List<SkillButton>();

        while (left != null || right != null)
        {
            if (left != null && collabsCount < 4 && left.attribute == attribute)
            {
                usesBtn.Add(left);
                left = left.leftBtn;
                collabsCount++;
            }
            else
            {
                left = null;
            }

            if (right != null && collabsCount < 4 && right.attribute == attribute)
            {
                usesBtn.Add(right);
                right = right.rightBtn;
                collabsCount++;
            }
            else
            {
                right = null;
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
