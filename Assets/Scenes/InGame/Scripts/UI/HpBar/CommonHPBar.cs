using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonHPBar : HpBarBase
{

    [Header("UI")]
    public Image hpBar;

    RectTransform canvasRt;
    RectTransform rect;


    public void InitCanvas(RectTransform _canvasRt)
    {
        canvasRt = _canvasRt;
        rect = GetComponent<RectTransform>();
    }

    public override void DestroyBar()
    {
        Destroy(gameObject);
    }

    public override void FollowPos(Vector3 pos)
    {
        pos.y -= 1f;
        var anchorPos = pos.WorldToRectPosition(canvasRt);
        rect.anchoredPosition = anchorPos;
    }

    public override void InitBar(float _maxHp)
    {
        maxHP = _maxHp;
    }

    public override void UpdateFill(float _hp)
    {
        hp = _hp;
        hpBar.fillAmount = hp / maxHP;
    }
}
