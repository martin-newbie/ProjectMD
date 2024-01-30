using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seongah : ActiveSkillBehaviour
{
    float passiveCool = 20f;
    float passiveCur = 0f;

    SeongahSkillBullet skillBullet;

    public Seongah(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
    {
        skillBullet = InGamePrefabsManager.GetObject("SeongahSkillBullet").GetComponent<SeongahSkillBullet>();
    }

    public override void UnitActive()
    {
        base.UnitActive();
        AddBuff(StatusType.CRI_DAMAGE, skillStatus.GetEnforceSkillValue(unitData.skill_level[2]), 0f);
    }

    public override void Update()
    {
        if (!nowActive) return;

        base.Update();

        passiveCur += Time.deltaTime;
        if(passiveCur >= passiveCool)
        {
            AddBuff(StatusType.DMG, skillStatus.GetPassiveSkillValue(unitData.skill_level[1]), 10f);
            passiveCur = 0f;
        }
    }

    protected override IEnumerator AttackLogic()
    {
        var target = GetPreferTarget();
        ShootBullet(target);

        yield return PlayAnimAndWait("battle_attack");
        yield return PlayAnimAndWait("battle_bolt");
        yield break;
    }

    public override IEnumerator ActiveSkill(SkillData skillData)
    {
        var target = GetPreferTarget();
        skillData.damageData.SetValue(StatusType.PENETRATE, skillData.collabseCount + 1);
        PlayAnimAndWait("battle_wait", true);

        yield return PlayAnimAndWait("active_skill");
        var bullet = Instantiate(skillBullet, GetBoneWorldPos("bullet_pos"), model.transform.rotation);
        bullet.InitBulletAndShoot(this, skillData.damageData, GetOpponentGroup(), GetTargetDir(target));
        yield return PlayAnimAndWait("battle_attack");
    }

    public override void CollabseBuff(SkillData skillData, UnitBehaviour subjectUnit)
    {
        skillData.damageData.AddIncreaseValue(StatusType.DMG, 8f);
    }

    public override bool GetActiveSkillCondition()
    {
        bool emenyExists = InGameManager.Instance.allUnits.FindAll((item) => item.group == GetOpponentGroup()).Count > 0;
        return emenyExists && base.GetActiveSkillCondition();
    }
}
