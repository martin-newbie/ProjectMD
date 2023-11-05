using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nina : UnitBehaviour
{
    public Nina(UnitObject _subject) : base(_subject)
    {
    }

    protected override IEnumerator MoveToTargetRange()
    {
        yield return PlayAnimAndWait("battle_move-wait");
        yield return base.MoveToTargetRange();
    }

    protected override IEnumerator AttackLogic()
    {
        for (int i = 0; i < 3; i++)
        {
            var target = GetOpponent();
            ShootBullet(target);
            PlayAnim("battle_attack");
            yield return new WaitForSeconds(0.15f);
        }
    }
}
