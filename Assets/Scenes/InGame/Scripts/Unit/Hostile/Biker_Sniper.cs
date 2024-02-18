using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biker_Sniper : UnitBehaviour
{
    public Biker_Sniper(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
    {
    }

    protected override IEnumerator AttackLogic()
    {
        var target = GetPreferTarget();
        ShootBullet(target);
        float delay = GetStatus(StatusType.RPM);
        yield return PlayAnimAndWait("battle_attack", false, delay / GetStatus(StatusType.ATK_TIMESCALE));
    }
}
