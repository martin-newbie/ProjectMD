using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lada : ActiveSkillBehaviour
{

    GameObject muzzleEffect;

    public Lada(UnitObject _subject) : base(_subject)
    {
        muzzleEffect = InGamePrefabsManager.GetObject("ShotGunMuzzle");
    }

    public override IEnumerator ActiveSkill(ActiveSkillValue skillData)
    {
        yield break;
    }

    public override bool ActiveSkillCondition()
    {
        return false;
    }

    public override void CollabseSkill(ActiveSkillValue skillData, UnitBehaviour subjectUnit)
    {
    }

    protected override IEnumerator AttackLogic()
    {
        var target = GetNearestOpponent();
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
