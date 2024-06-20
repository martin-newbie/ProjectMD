using System;
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
        btnTemp.rect.anchoredPosition = btnStartPos;
        btnTemp.SetData(_linkedData);
        activatingBtn.Add(btnTemp);

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

        AlignSkillButton();
    }

    public void RemoveButtonAt(int idx)
    {
        var item = activatingBtn[idx];
        activatingBtn.Remove(item);
        PushButton(item);
        AlignSkillButton();
    }

    public void SetSkillButtonChain(int idx, int chain)
    {
        activatingBtn[idx].SetChainedData(chain);
    }

    public void CollabseEffect(int originIdx, int leftIdx, int leftCount, int rightIdx, int rightCount, Action actionComplete = null)
    {
        skillBlur.SetActive(true);
        List<SkillButton> buttons = new List<SkillButton>();

        var originButton = activatingBtn[originIdx];
        if (leftCount > 0)
            buttons.AddRange(activatingBtn.GetRange(leftIdx, leftCount));
        if (rightCount > 0)
            buttons.AddRange(activatingBtn.GetRange(rightIdx, rightCount));
        activatingBtn.RemoveAll(item => buttons.Contains(item));

        StartCoroutine(moveRoutine());

        IEnumerator moveRoutine()
        {
            Coroutine routine = null;
            foreach (var item in buttons)
            {
                item.transform.SetAsFirstSibling();
                routine = item.MovePos(item.rect.anchoredPosition, originButton.rect.anchoredPosition, () => Time.unscaledDeltaTime, EaseOutSin);
            }

            yield return routine;

            actionComplete?.Invoke();
            skillBlur.SetActive(false);
            foreach (var item in buttons)
            {
                PushButton(item);
            }
            AlignSkillButton();
        }
    }

    void PushButton(SkillButton item)
    {
        item.ClearButton();
        item.gameObject.SetActive(false);
        skillBtnPool.Push(item);
    }

    void AlignSkillButton()
    {
        for (int i = 0; i < activatingBtn.Count; i++)
        {
            activatingBtn[i].SetIdx(i);
            activatingBtn[i].MovePos(activatingBtn[i].rect.anchoredPosition, GetButtonPos(i), () => Time.deltaTime, EaseOutBack);
        }
    }

    Vector3 GetButtonPos(int idx)
    {
        return new Vector3((175f / 2f + 5) + (5 + 175) * idx, 0);
    }

    float EaseOutBack(float x)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1;
        float t = 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2);
        return t;
    }

    float EaseOutSin(float x)
    {
        return Mathf.Sin((x * Mathf.PI) / 2);
    }
}
