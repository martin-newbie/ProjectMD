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
    int lineCount;

    float visualedHp;

    public override void DestroyBar()
    {
    }

    public override void FollowPos(Vector3 pos)
    {
    }

    public override void InitBar(float _maxHp)
    {
        maxHP = _maxHp;

        lineCount = (int)(maxHP / lineValue);
        visualedHp = lineValue - 0.001f;
    }

    public override void UpdateFill(float _hp)
    {
        curHp = _hp;
    }

    void Update()
    {
        visualedHp = Mathf.Lerp(curHp, visualedHp, Time.deltaTime * 100f);

        float calcHp = visualedHp % lineValue;
        filledHpBar.fillAmount = calcHp / lineValue;

        int curLine = (int)(visualedHp / maxHP);
        int visualIdx = (curLine + barColors.Length) % barColors.Length;
        int backIdx = (visualIdx - 1 + barColors.Length) % barColors.Length;

        filledHpBar.color = barColors[visualIdx];
        backHpBar.color = barColors[backIdx];

        remainLineText.text = string.Format("x{0}", curLine);
    }
}
