using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seongah : UnitBehaviour
{
    public Seongah(UnitObject _subject) : base(_subject)
    {

    }

    protected override IEnumerator AttackLogic()
    {
        // shoot bullet
        yield return PlayAnimAndWait("battle_attack");
        yield return PlayAnimAndWait("battle_bolt");
        yield break;
    }
}
