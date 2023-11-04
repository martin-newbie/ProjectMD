using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBehaviour : UnitBehaviour
{
    public TestBehaviour(UnitObject _subject) : base(_subject)
    {

    }

    protected override IEnumerator AttackToTarget()
    {
        yield break;
    }
}
