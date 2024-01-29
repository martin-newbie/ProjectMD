using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lada : ActiveSkillBehaviour
{

    GameObject muzzleEffect;

    public Lada(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
    {
        muzzleEffect = InGamePrefabsManager.GetObject("ShotGunMuzzle");
    }

    public override IEnumerator ActiveSkill(SkillData skillData)
    {
        yield break;
    }

    public override bool GetActiveSkillCondition()
    {
        return false;
    }

    public override void CollabseBuff(SkillData skillData, UnitBehaviour subjectUnit)
    {
    }

    protected override IEnumerator AttackLogic()
    {
        var target = GetPreferTarget();
        Instantiate(muzzleEffect, GetBoneWorldPos("bullet_pos"), model.transform.rotation);
        yield return PlayAnimAndWait("battle_attack");
        for (int i = 0; i < 3; i++)
        {
            target?.OnDamage(GetDamageStruct(), this);
        }
        yield return PlayAnimAndWait("battle_pump");
        yield break;
    }

    protected override IEnumerator Reload()
    {
        yield return PlayAnimAndWait("battle_reload_1");
        for (int i = 0; i < maxAmmo; i++)
        {
            curAmmo++;
            yield return PlayAnimAndWait("battle_reload_2");
        }
        yield return PlayAnimAndWait("battle_reload_3");
    }
}
