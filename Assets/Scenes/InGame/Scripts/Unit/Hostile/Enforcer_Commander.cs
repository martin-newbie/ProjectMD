using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enforcer_Commander : UnitBehaviour
{
    public Enforcer_Commander(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
    {

    }

    protected override IEnumerator MoveToTargetRange()
    {
        PlayAnim("battle_start", false);
        yield return PlayAnimAndWait("battle_car_start", false, GetAnimTime("battle_car_start"), 1);
        PlayAnim("battle_car_move", true, 1);
        PlayAnim("battle_move", true);
        yield return StartCoroutine(CombatMoveLogic());
        PlayAnim("battle_wait", true);
        PlayAnim("battle_car_wait", true, 1);
    }

    protected override IEnumerator AttackToTarget()
    {
        yield return StartCoroutine(AttackLogic());
    }

    protected override IEnumerator AttackLogic()
    {
        int rand = Random.Range(0, 2);
        if(rand == 0) yield return StartActionCoroutine(Attack1Logic());
        else yield return StartActionCoroutine(Attack2Logic());
        yield break;
    }

    IEnumerator Attack1Logic()
    {
        yield return PlayAnimAndWait("battle_attack1_start");
        // spawn enemy
        yield return PlayAnimAndWait("battle_attack1_speak");
        yield return PlayAnimAndWait("battle_attack1_end");
        yield break;
    }

    IEnumerator Attack2Logic()
    {
        yield return PlayAnimAndWait("battle_attack2_start");
        yield return CommonBurstFire(3);
        yield return PlayAnimAndWait("battle_attack2_end");
    }

    protected override IEnumerator CommonBurstFire(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var target = GetPreferTarget();
            ShootBullet(target);
            float delay = GetStatus(StatusType.RPM);
            yield return PlayAnimAndWait("battle_attack2_shot", false, delay / GetStatus(StatusType.ATK_TIMESCALE));
        }
    }

    protected override void ShootBullet(Entity target, string key = "bullet_pos")
    {
        base.ShootBullet(target, key);
        curAmmo = maxAmmo;
    }

    protected override void OnRetire()
    {
        PlayAnim("battle_car_retire", false, 1);
        base.OnRetire();
    }
}
