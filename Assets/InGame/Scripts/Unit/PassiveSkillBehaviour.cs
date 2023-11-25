using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveSkillBehaviour : UnitBehaviour
{

    protected float curPassiveCool;

    protected PassiveSkillBehaviour(UnitObject _subject) : base(_subject)
    {
        // get passive skill value
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