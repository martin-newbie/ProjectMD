using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zenkyoto_Shotgun : UnitBehaviour
{
    public Zenkyoto_Shotgun(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
    {
    }

    protected override IEnumerator AttackLogic()
    {
        var target = GetPreferTarget();
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
        yield return PlayAnimAndWait("battle_reload/battle_reload1");
        for (int i = 0; i < maxAmmo; i++)
        {
            curAmmo++;
            yield return PlayAnimAndWait("battle_reload/battle_reload2");
        }
        yield return PlayAnimAndWait("battle_reload/battle_reload3");
    }
}
