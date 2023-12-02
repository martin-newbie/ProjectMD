using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonHPBar : HpBarBase
{

    [Header("UI")]
    public Image hpBar;
    RectTransform rect;

    public float hp { get; set; }
    public float maxHP { get; set; }

    public override void DestroyBar()
    {
        Destroy(gameObject);
    }

    public override void FollowPos(Vector3 pos)
    {
        pos.y -= 1f;
        var anchorPos = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
        rect.anchoredPosition = anchorPos;
    }

    public override void InitBar(float _maxHp)
    {
        maxHP = _maxHp;
        rect = GetComponent<RectTransform>();
    }

    public override void UpdateFill(float _hp)
    {
        hp = _hp;
        hpBar.fillAmount = hp / maxHP;
    }
}
