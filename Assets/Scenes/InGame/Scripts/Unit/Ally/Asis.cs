using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asis : ActiveSkillBehaviour
{

    Explosion explosion;
    BigShotgunMuzzle skillEffect;

    public Asis(UnitObject _subject) : base(_subject)
    {
        explosion = InGamePrefabsManager.GetObject("AsisCommonAttackExplosion").GetComponent<Explosion>();
        skillEffect = InGamePrefabsManager.GetObject("AsisSkillEffect").GetComponent<BigShotgunMuzzle>();
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
        bullet.StartBulletEffect(startPos, targetPos, 5f, () => OnCompleteBulletMove(targetPos), 1);
        curAmmo--;
    }

    private void OnCompleteBulletMove(Vector3 targetPos)
    {
        var obj = Instantiate(explosion, targetPos, Quaternion.identity);
        obj.StartExplosion(GetOpponentGroup(), GetDamageStruct(), this);
    }

    public override void CollabseBuff(DamageStruct skillData, UnitBehaviour subjectUnit)
    {

    }

    public override IEnumerator ActiveSkill(DamageStruct skillData)
    {
        yield return StartActionCoroutine(ActiveSkillAction(skillData));
        yield break;
    }

    IEnumerator ActiveSkillAction(DamageStruct skillData)
    {
        var target = GetNearestOpponent();
        Instantiate(skillEffect, GetBoneWorldPos("bullet_pos"), Quaternion.Euler(0, 0, 90 * GetTargetDir(target) * -1)).StartMuzzle(this, skillData, GetOpponentGroup());
        yield return PlayAnimAndWait("active_skill");
        AddBuff(StatusType.DEF, 0, 20);
        yield return PlayAnimAndWait("battle_reload");
        curAmmo = maxAmmo;
        yield break;
    }

    public override bool ActiveSkillCondition()
    {
        var hostile = GetNearestOpponent();

        if (IsInsideRange(hostile) && (state == BehaviourState.ACTING || state == BehaviourState.INCOMBAT))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
