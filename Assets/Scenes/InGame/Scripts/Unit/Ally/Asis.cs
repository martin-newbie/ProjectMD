using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asis : ActiveSkillBehaviour
{

    Explosion explosion;
    BigShotgunMuzzle skillEffect;

    public Asis(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
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
        var target = GetPreferTarget();
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

    public override void CollabseBuff(SkillData skillData, UnitBehaviour subjectUnit)
    {

    }

    public override SkillData GetDefaultSkillValue()
    {
        var value = base.GetDefaultSkillValue();
        value.damageData.AddIncreaseValue(StatusType.DMG, skillStatus.GetActiveSkillValue(1));
        return value;
    }

    public override IEnumerator ActiveSkill(SkillData skillData)
    {
        var target = GetPreferTarget();
        int atkDir = GetTargetDir(target);
        Instantiate(skillEffect, new Vector3(atkDir * 1f, 0.8f, 0) + transform.position, Quaternion.Euler(0, 0, 90 * atkDir * -1)).StartMuzzle(this, skillData.damageData, GetOpponentGroup());
        yield return PlayAnimAndWait("active_skill");
        AddBuff(StatusType.DEF, 0, 20);
        yield return PlayAnimAndWait("battle_reload");
        curAmmo = maxAmmo;
        yield break;
    }

    public override bool GetActiveSkillCondition()
    {
        var hostile = GetPreferTarget();
        return hostile != null && IsInsideRange(hostile) && base.GetActiveSkillCondition();
    }
}
