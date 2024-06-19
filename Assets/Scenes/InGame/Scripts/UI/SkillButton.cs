using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField] Image collabseImage;
    [SerializeField] Image profileImage;
    [SerializeField] Text costText;
    [SerializeField] Text chainText;
    [SerializeField] GameObject lockedObj;
    [HideInInspector] public RectTransform rect;
    
    SkillBehaviour linkedData;
    int buttonIdx = -1;

    SkillCanvas manager;
    bool pointerEnter;

    bool isTouch;
    Vector3 startPos, endPos;

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
        lockedObj.SetActive(false);
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
            float dur = 1f;
            float timer = 0f;

            while (timer < dur)
            {
                float c1 = 1.70158f;
                float c3 = c1 + 1;
                float t = 1 + c3 * Mathf.Pow((timer / dur) - 1, 3) + c1 * Mathf.Pow((timer / dur) - 1, 2);
                rect.anchoredPosition = Vector3.LerpUnclamped(start, target, t);
                timer += Time.deltaTime;
                yield return null;
            }

            rect.anchoredPosition = target;
        }
    }

    public void SetChainedData(int chainCount)
    {
        lockedObj.SetActive(true);
        chainText.text = chainCount.ToString();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isTouch) return;

        endPos = eventData.position;
        if (pointerEnter)
        {
            if(startPos.y < endPos.y)
            {
                // use skill
            }
        }
        else
        {
            // collabse skill
        }

        isTouch = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPos = eventData.position;
        isTouch = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerEnter = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerEnter = true;
    }
}
