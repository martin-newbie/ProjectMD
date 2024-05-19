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
    [SerializeField] Image outlineProgress;
    [SerializeField] GameObject outlineSelected;
    [HideInInspector] public RectTransform rect;
    
    SkillBehaviour linkedData;
    Animator anim;
    int buttonIdx = -1;

    SkillCanvas manager;
    bool pointerEnter;

    public void InitButton(SkillCanvas _manager)
    {
        manager = _manager;
        rect = GetComponent<RectTransform>();
        anim = GetComponent<Animator>();
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

    public void SetProgress(float amount)
    {
        outlineProgress.fillAmount = amount;
    }

    public void SelectButton()
    {
        outlineSelected.SetActive(true);
        anim.SetTrigger("hopping");
        SetProgress(1f);
    }

    public void DeselectButton()
    {
        outlineSelected.SetActive(false);
        SetProgress(0f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!pointerEnter)
        {
            PlayableGamePlayer.Instance.CancelSkill();
            return;
        }
        pointerEnter = false;
        PlayableGamePlayer.Instance.UseSkill();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PlayableGamePlayer.Instance.StartSkill(buttonIdx);
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
