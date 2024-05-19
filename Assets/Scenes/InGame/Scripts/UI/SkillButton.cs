using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] Image collabseImage;
    [SerializeField] Image profileImage;
    [SerializeField] Text costText;
    [HideInInspector] public RectTransform rect;
    
    SkillBehaviour linkedData;
    int buttonIdx = -1;

    SkillCanvas manager;


    public void InitButton(SkillCanvas _manager)
    {
        manager = _manager;
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (linkedData == null) return;
        costText.text = linkedData.cost.ToString();
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

    public void SetData(SkillBehaviour _linkedData)
    {
        linkedData = _linkedData;
        profileImage.sprite = ResourceManager.GetUnitProfile(linkedData.unitData.index);
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

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    //public void UseSkill()
    //{
    //    if (!linkedData.GetActiveSkillCondition())
    //    {
    //        // 빨강 오류 마크 점등
    //        return;
    //    }
    //    PlayableGamePlayer.Instance.UseSkill(buttonIdx);
    //}
}
