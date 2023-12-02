using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummy : UnitBehaviour
{
    public TrainingDummy(UnitObject _subject) : base(_subject)
    {
        maxHp = 100000;
        hp = maxHp;
    }

    protected override void InCombatFunc()
    {
        return;
    }

    protected override IEnumerator AttackLogic()
    {
        yield break;
    }
}
