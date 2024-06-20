using System;
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

    SkillCanvas manager;
    SkillBehaviour linkedData;
    int buttonIdx = -1;

    bool pointerEnter;

    bool isTouch;
    Vector3 startPos, endPos;
    Coroutine routine;

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

    public Coroutine MovePos(Vector3 start, Vector3 target, float deltaTime, Func<float, float> ease)
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(moveRoutine());
        return routine;

        IEnumerator moveRoutine()
        {
            float dur = 0.7f;
            float timer = 0f;

            while (timer < dur)
            {
                rect.anchoredPosition = Vector3.LerpUnclamped(start, target, ease.Invoke(timer / dur));
                timer += deltaTime;
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
        manager.skillBlur.SetActive(false);
        Time.timeScale = 1f;

        endPos = eventData.position;
        if (pointerEnter)
        {
            PlayableGamePlayer.Instance.CollabseSkill(buttonIdx);
        }
        else
        {
            if (startPos.y < endPos.y)
            {
                PlayableGamePlayer.Instance.UseSkill(buttonIdx);
            }
        }

        isTouch = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPos = eventData.position;
        isTouch = true;
        manager.skillBlur.SetActive(true);
        Time.timeScale = 0.1f;
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
