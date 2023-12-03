using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : HpBarBase
{
    [SerializeField] Text hpRemainText;
    [SerializeField] Text remainLineText;
    [SerializeField] Image filledHpBar;
    [SerializeField] Image backHpBar;

    [SerializeField] Color[] barColors = new Color[5];
    float lineValue = 100;

    float maxHp;
    float curHp;

    float visualedHp;
    float distSpeed;

    public override void DestroyBar()
    {
        gameObject.SetActive(false);
    }

    public override void FollowPos(Vector3 pos)
    {
    }

    public override void InitBar(float _maxHp)
    {
        maxHP = _maxHp;
        visualedHp = lineValue - 0.001f;
    }

    public override void UpdateFill(float _hp)
    {
        curHp = _hp;
        distSpeed = Mathf.Abs(visualedHp - curHp) / curHp * 1000f;
    }

    void Update()
    {
        visualedHp = Mathf.Lerp(visualedHp, curHp, distSpeed * Time.deltaTime);

        float calcHp = visualedHp % lineValue;
        filledHpBar.fillAmount = calcHp / lineValue;

        int curLine = (int)(visualedHp / lineValue);
        int visualIdx = (curLine + barColors.Length) % barColors.Length;
        int backIdx = (visualIdx - 1 + barColors.Length) % barColors.Length;

        filledHpBar.color = barColors[visualIdx];
        backHpBar.color = barColors[backIdx];

        remainLineText.text = string.Format("x{0}", curLine);
    }
}
