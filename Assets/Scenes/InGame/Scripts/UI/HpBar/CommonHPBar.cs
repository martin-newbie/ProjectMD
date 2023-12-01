using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonHPBar : MonoBehaviour, IHealthBar
{

    [Header("UI")]
    public Image hpBar;

    public float hp { get; set; }
    public float maxHP { get; set; }

    RectTransform parentRect;

    public void InitHpBarUI(RectTransform parent)
    {
        parentRect = parent;
    }

    public void DestroyBar()
    {
        Destroy(gameObject);
    }

    public void FollowPos(Vector3 pos)
    {
        pos.y -= 1f;
        RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
    }

    public void InitBar(float _maxHp)
    {
        maxHP = _maxHp;
    }

    public void SetBar(float _hp)
    {
        hp = _hp;
        hpBar.fillAmount = hp / maxHP;
    }
}
