using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveAbleBehaviour : UnitBehaviour
{

    protected float curPassiveCool;

    protected PassiveAbleBehaviour(UnitObject _subject) : base(_subject)
    {
    }

    protected override void InCombatFunc()
    {
        if (PassiveSkillCondition())
        {
            StartActionCoroutine(PassiveSkillActive());
        }
        else
        {
            base.InCombatFunc();
        }
    }

    abstract protected bool PassiveSkillCondition();
    abstract protected IEnumerator PassiveSkillActive();
}
