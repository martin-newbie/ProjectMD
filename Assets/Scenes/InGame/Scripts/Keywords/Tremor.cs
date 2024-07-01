using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tremor : IKeyword, IKeywordOnSecond, IKeywordOnDamage
{
    public KeywordType id => KeywordType.TREMOR;

    public int count { get; set; }

    public Tremor(int count)
    {
        this.count = count;
    }

    public void OnDamage(DamageStruct damage, Entity from, Entity target)
    {
        if (count <= 0) return;
        var extraDmg = damage.GetValue(StatusType.DMG) * 0.2f;
        target.OnDamageForce(extraDmg, from);
        count--;
    }

    public void OnSecond(UnitBehaviour target)
    {
        if(count <= 0) return;
        count--;
    }
}
