using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillCanvas : MonoBehaviour
{

    public GameObject skillBlur;

    [Header("Skill Button")]
    public SkillButton skillButtonPrefab;
    public Transform buttonParent;
    Stack<SkillButton> skillBtnPool = new Stack<SkillButton>();
    [HideInInspector] public List<SkillButton> activatingBtn = new List<SkillButton>();

    Vector3 btnStartPos = new Vector3(1900f, 0f);
    int curSkillCount;

    [Header("Cost")]
    public SkillCost skillCost;

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

    public void AddSkillButton(SkillBehaviour _linkedData)
    {
        var btnTemp = skillBtnPool.Pop();
        btnTemp.transform.SetAsLastSibling();
        btnTemp.gameObject.SetActive(true);
        btnTemp.MovePos(btnStartPos, GetButtonPos(curSkillCount));
        btnTemp.SetData(_linkedData);
        activatingBtn.Add(btnTemp);

        curSkillCount++;
        AlignSkillButton();
    }

    public void RemoveButtonRange(int idx, int count)
    {
        var btns = activatingBtn.GetRange(idx, count);
        activatingBtn.RemoveRange(idx, count);
        foreach (var btn in btns)
        {
            btn.ClearButton();
            btn.gameObject.SetActive(false);
            skillBtnPool.Push(btn);
        }

        curSkillCount -= count;
        AlignSkillButton();
    }

    public void RemoveButtonAt(int idx)
    {
        var btnTemp = activatingBtn[idx];
        btnTemp.ClearButton();
        btnTemp.gameObject.SetActive(false);
        activatingBtn.Remove(btnTemp);
        skillBtnPool.Push(btnTemp);

        curSkillCount--;
        AlignSkillButton();
    }

    public void SetSkillButtonChain(int idx, int chain)
    {
        activatingBtn[idx].SetChainedData(chain);
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
        return new Vector3((175f / 2f + 5) + (5 + 175) * idx, 0);
    }
}
