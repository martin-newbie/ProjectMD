using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField] Image inImage;
    [HideInInspector] public RectTransform rect;
    
    ActiveAbleBehaviour linkedData;
    int buttonIdx = -1;

    SkillCanvas manager;


    public void InitButton(SkillCanvas _manager)
    {
        manager = _manager;
        rect = GetComponent<RectTransform>();
    }

    public void ClearButton()
    {
        buttonIdx = -1;
        linkedData = null;
    }

    public void SetIdx(int idx)
    {
        buttonIdx = idx;
    }

    public void SetData(ActiveAbleBehaviour _linkedData)
    {
        linkedData = _linkedData;
        // set sprite
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
        SkillManager.Instance.UseSkill(buttonIdx);
    }
}
