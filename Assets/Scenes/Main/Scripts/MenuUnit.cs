using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUnit : MonoBehaviour
{
    public SkeletonGraphic skeleton;

    RectTransform parentRect;
    RectTransform rectTransform;
    bool isDrag;


    public void InitButton(RectTransform _parentRect)
    {
        parentRect = _parentRect;
        rectTransform = GetComponent<RectTransform>();
    }

    public void UpdateUnitIdx(int idx)
    {
        skeleton.UpdateSkeleton(ResourceManager.GetSkeleton(idx));
        PlayAni("wait");
    }

    void PlayAni(string key, bool loop = false)
    {
        skeleton.AnimationState.SetAnimation(0, key, loop);
    }

    private void Update()
    {
        if (isDrag)
        {
            var anchorPos = Input.mousePosition.MouseToRectPosition(parentRect);
            rectTransform.anchoredPosition = anchorPos;
        }
        else
        {
            // random move

        }
    }

    public void OnPointerDown()
    {
        isDrag = true;
        PlayAni("drag");
    }

    public void OnPointerUp()
    {
        isDrag = false;

        var anchorPos = rectTransform.anchoredPosition;
        anchorPos.y = -130;
        rectTransform.anchoredPosition = anchorPos;

        PlayAni("wait");
    }
}
