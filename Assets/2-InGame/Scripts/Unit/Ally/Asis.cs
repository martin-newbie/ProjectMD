using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asis : ActiveSkillBehaviour
{
    public Asis(UnitObject _subject) : base(_subject)
    {
        maxAmmo = 1;
        curAmmo = maxAmmo;
    }

    protected override IEnumerator MoveToTargetRange()
    {
        yield return PlayAnimAndWait("wp_turn");
        yield return base.MoveToTargetRange();
    }

    protected override IEnumerator AttackLogic()
    {
        var target = GetNearestOpponent();
        ShootBullet(target);
        yield return PlayAnimAndWait("battle_attack");
    }

    protected override void ShootBullet(UnitBehaviour target, string key = "bullet_pos")
    {
        if (target == null)
        {
            return;
        }

        var startPos = GetBoneWorldPos(key);
        var targetPos = target.transform.position;

        var bullet = Instantiate(probBullet, startPos, Quaternion.identity);
        bullet.GetComponent<SpriteRenderer>().sprite = InGameSpriteManager.Instance.asisGrenadeSprite;
        bullet.StartBulletEffect(startPos, targetPos, 5f, () => target.OnDamage(damage, this), 1);
        curAmmo--;
    }

    public override void CollabseSkill(ActiveSkillValue skillData, UnitBehaviour subjectUnit)
    {

    }

    public override IEnumerator ActiveSkill(ActiveSkillValue skillData)
    {
        yield break;
    }

    protected override bool PassiveSkillCondition()
    {
        return false;
    }

    protected override IEnumerator PassiveSkillActive()
    {
        yield break;
    }
}
