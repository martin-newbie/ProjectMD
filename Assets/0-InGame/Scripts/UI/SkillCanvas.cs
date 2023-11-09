using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillCanvas : MonoBehaviour
{
    public SkillButton skillButtonPrefab;
    public Transform buttonParent;
    Stack<SkillButton> skillBtnPool = new Stack<SkillButton>();
    List<SkillButton> curSkills = new List<SkillButton>();

    Vector3 btnStartPos = new Vector3(2812.5f, -150f);
    int curSkillCount;

    private void Start()
    {
        // init

        for (int i = 0; i < 10; i++)
        {
            var btnTemp = Instantiate(skillButtonPrefab, buttonParent);
            btnTemp.InitButton(this);
            btnTemp.rect.anchoredPosition = GetButtonPos(i);
            btnTemp.gameObject.SetActive(false);
            skillBtnPool.Push(btnTemp);
        }
    }

    public void AddSkillButton()
    {
        if (skillBtnPool.Count == 0) return;
        if (curSkillCount >= 10) return;

        var btnTemp = skillBtnPool.Pop();
        btnTemp.gameObject.SetActive(true);
        btnTemp.MovePos(btnStartPos, GetButtonPos(curSkillCount));
        btnTemp.SetData(Random.Range(0, 6));

        if(curSkillCount > 0)
        {
            var last = curSkills.Last();
            btnTemp.leftBtn = last;
            last.rightBtn = btnTemp;
        }

        curSkills.Add(btnTemp);
        curSkillCount++;

    }

    public void PushSkillButton(SkillButton btn)
    {
        var left = btn.leftBtn;
        var right = btn.rightBtn;

        if(left != null)
            left.rightBtn = right;
        if (right != null)
            right.leftBtn = left;

        curSkills.Remove(btn);
        btn.rect.anchoredPosition = btnStartPos;
        btn.gameObject.SetActive(false);
        skillBtnPool.Push(btn);

        curSkillCount--;

        AlignSkillButton();
    }

    void AlignSkillButton()
    {
        for (int i = 0; i < curSkillCount; i++)
        {
            var item = curSkills[i];
            item.MovePos(item.rect.anchoredPosition3D, GetButtonPos(i));
        }
    }

    Vector3 GetButtonPos(int idx)
    {
        return new Vector3(162.5f + (40 + 225) * idx, -150);
    }
}
