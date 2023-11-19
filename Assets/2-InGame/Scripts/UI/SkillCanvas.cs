using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillCanvas : MonoBehaviour
{
    public SkillButton skillButtonPrefab;
    public Transform buttonParent;
    Stack<SkillButton> skillBtnPool = new Stack<SkillButton>();
    [HideInInspector] public List<SkillButton> activatingBtn = new List<SkillButton>();

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

    public void AddSkillButton(ActiveSkillBehaviour _linkedData)
    {
        var btnTemp = skillBtnPool.Pop();
        btnTemp.gameObject.SetActive(true);
        btnTemp.MovePos(btnStartPos, GetButtonPos(curSkillCount));
        btnTemp.SetData(_linkedData);
        activatingBtn.Add(btnTemp);

        curSkillCount++;
        AlignSkillButton();
    }

    public void RemoveButtonAt(int idx)
    {
        var btnTemp = activatingBtn[idx];
        btnTemp.gameObject.SetActive(false);
        activatingBtn.Remove(btnTemp);
        skillBtnPool.Push(btnTemp);

        curSkillCount--;
        AlignSkillButton();
    }

    void AlignSkillButton()
    {
        for (int i = 0; i < activatingBtn.Count; i++)
        {
            activatingBtn[i].SetIdx(i);
            activatingBtn[i].MovePos(activatingBtn[i].rect.anchoredPosition, GetButtonPos(i));
        }
    }

    Vector3 GetButtonPos(int idx)
    {
        return new Vector3(162.5f + (40 + 225) * idx, -150);
    }
}
