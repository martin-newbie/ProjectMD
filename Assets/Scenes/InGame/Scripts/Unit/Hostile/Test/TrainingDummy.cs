using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummy : UnitBehaviour
{
    public TrainingDummy(UnitObject _subject) : base(_subject)
    {
    }

    public override void InitCommon(int idx, int level, int barType)
    {
        base.InitCommon(idx, level, barType);
        hp = 100000;
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
