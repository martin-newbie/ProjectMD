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
    int lineCount;

    float curHp;

    float visualedHp;

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
        visualedHp = 0;
        lineCount = Mathf.FloorToInt(Mathf.Sqrt(Mathf.Sqrt(maxHP)));
        lineValue = maxHP / lineCount;
    }

    public override void UpdateFill(float _hp)
    {
        curHp = _hp;
    }

    void Update()
    {
        visualedHp = Mathf.Lerp(visualedHp, curHp, Time.deltaTime * 20f);

        int curLine = Mathf.FloorToInt(curHp / lineValue);
        float visualFill = (visualedHp % curLine) / lineValue;

        filledHpBar.fillAmount = visualFill;
        filledHpBar.color = barColors[curLine % 5];
        backHpBar.color = curLine == 0 ? Color.black : barColors[(curLine - 1) % 5];
        remainLineText.text = string.Format("x{0}", curLine);
    }
}
