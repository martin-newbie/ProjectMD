using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minel : UnitBehaviour
{
    public Minel(UnitObject _subject) : base(_subject)
    {
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
