using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveAbleBehaviour : PassiveAbleBehaviour
{
    protected ActiveAbleBehaviour(UnitObject _subject) : base(_subject)
    {
        // get active skill value
    }
    
    public abstract void CollabseSkill(SkillValue skillData, UnitBehaviour subjectUnit);
    public abstract IEnumerator ActiveSkill();
}


public class SkillValue
{

}