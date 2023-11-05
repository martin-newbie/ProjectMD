using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asis : UnitBehaviour
{
    public Asis(UnitObject _subject) : base(_subject)
    {
    }

    protected override IEnumerator MoveToTargetRange()
    {
        yield return PlayAnimAndWait("wp_turn");
        yield return base.MoveToTargetRange();
    }

    protected override IEnumerator AttackLogic()
    {
        // shoot grenade
        yield return PlayAnimAndWait("battle_attack");
    }
}
