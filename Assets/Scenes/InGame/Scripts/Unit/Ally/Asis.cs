using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asis : SkillBehaviour
{
    float passiveCoolTime = 20f;
    float passiveCoolCur = 0f;
    float passiveDur = 3f;
    float passiveCur = 0f;

    Explosion explosion;
    BigShotgunMuzzle skillEffect;
    Vector3 targetAimPos;
    Grenade grenadeBullet;

    public Asis(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
    {
        explosion = InGamePrefabsManager.GetObject("AsisCommonAttackExplosion").GetComponent<Explosion>();
        skillEffect = InGamePrefabsManager.GetObject("AsisSkillEffect").GetComponent<BigShotgunMuzzle>();
        grenadeBullet = InGamePrefabsManager.GetObject("Grenade").GetComponent<Grenade>();
    }

    public override void UnitActive()
    {
        base.UnitActive();
    }

    public override void Update()
    {
        if (!nowActive) return;

        base.Update();

        passiveCoolCur += Time.deltaTime;
        passiveCur += Time.deltaTime;
        if (passiveCoolCur >= passiveCoolTime && passiveCur >= passiveDur)
        {
            OnHeal(hp * 0.05f, this);
            passiveCoolCur = 0f;
            passiveCur = 0f;
        }
    }

    public override void OnHeal(float value, UnitBehaviour from)
    {
        base.OnHeal(value, from);

        if (Random.Range(0f, 1f) <= 0.25f)
        {
            string buffKey = "asis_sub_skill_buff";
            if (!buffTimers.Exists(item => item.key == buffKey))
            {
                AddBuff(StatusType.DEF, 8f, 25, buffKey);
            }
        }
    }

    protected override IEnumerator AttackAim()
    {
        var target = GetPreferTarget();
        SetModelRotByTarget(target);
        targetAimPos = target.transform.position;

        float aimingTime = GetAnimTime("battle_aiming");
        yield return PlayAnimAndWait("battle_aiming", false, aimingTime / GetStatus(StatusType.ATK_TIMESCALE));
    }

    protected override IEnumerator AttackToTarget()
    {
        return base.AttackToTarget();
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

        var grenade = Instantiate(grenadeBullet, startPos, Quaternion.identity);
        grenade.ShootGrenade(startPos, targetAimPos, () =>
        {
            OnCompleteBulletMove(targetAimPos);
        });
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
        value.damageData.AddIncreaseValue(StatusType.DMG, skillStatus.GetActiveValue(1));
        return value;
    }

    public override IEnumerator ActiveSkill(SkillData skillData)
    {
        AddBuff(StatusType.DEF, 0, 20);

        float debuffAmount = skillData.collabseCount * 3f;
        float debuffTime = 3f;
        var target = GetPreferTarget();
        target.OnDamage(skillData.damageData, this);
        target.AddDebuff(StatusType.DMG, debuffAmount, debuffTime);

        int atkDir = GetTargetDir(target);
        Instantiate(skillEffect, new Vector3(atkDir * 1f, 0.8f, 0) + transform.position, Quaternion.Euler(0, 0, 90 * atkDir * -1)).StartMuzzle();

        yield return PlayAnimAndWait("active_skill");
        curAmmo = maxAmmo;
        yield return PlayAnimAndWait("battle_reload");
        yield break;
    }

    public override bool GetActiveSkillCondition()
    {
        var hostile = GetPreferTarget();
        return hostile != null && IsInsideRange(hostile) && base.GetActiveSkillCondition();
    }

    protected override void GetDamage(float damage)
    {
        base.GetDamage(damage);
        passiveCur = 0f;
    }
}
