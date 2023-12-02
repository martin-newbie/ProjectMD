using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformUtil
{
    public static Vector3 WorldToRectPosition(this Vector3 pos, RectTransform rect)
    {
        Vector2 viewPos = Camera.main.WorldToViewportPoint(pos);
        Vector2 anchorPos = new Vector2(
            ((viewPos.x * rect.sizeDelta.x) - (rect.sizeDelta.x * 0.5f)),
            ((viewPos.y * rect.sizeDelta.y) - (rect.sizeDelta.y * 0.5f)));

        return anchorPos;
    }
}
