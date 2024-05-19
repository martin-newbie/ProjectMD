using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Picardy : SkillBehaviour
{

    PicardySkillSyringe skillBulletPrefab;

    public Picardy(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
    {
        skillBulletPrefab = InGamePrefabsManager.GetObject("PicardySyringe").GetComponent<PicardySkillSyringe>();
    }

    public override void UnitActive()
    {
        base.UnitActive();
    }

    public override IEnumerator ActiveSkill(SkillData skillData)
    {
        var target = InGameManager.Instance.allUnits.FindAll(item => item.group == group && item != this).OrderBy(item => item.hpAmount).ElementAt(0);
        SetModelRotByTarget(target);

        yield return PlayAnimAndWait("active_skill_aiming");
        skillData.damageData.SetValue(StatusType.DEF, GetStatus(StatusType.DEF));
        float healAmount = skillData.damageData.GetValue(StatusType.DEF);

        // shoot hp bullet to target
        var bulletPos = GetBoneWorldPos("bullet_pos");
        var skillBullet = Instantiate(skillBulletPrefab, bulletPos, Quaternion.identity);
        skillBullet.ObjectMove(bulletPos, target.GetBoneWorldPos("body"), () =>
        {
            target.OnHeal(healAmount, this);
        });
        yield return PlayAnimAndWait("active_skill_attack");
        yield return PlayAnimAndWait("active_skill_disarm");

    }

    public override bool GetActiveSkillCondition()
    {
        bool friendlyExist = InGameManager.Instance.allUnits.FindAll(item => item.group == group && item != this).Count > 0;
        return friendlyExist && base.GetActiveSkillCondition();
    }

    public override void CollabseBuff(SkillData skillData, UnitBehaviour subjectUnit)
    {

    }

    protected override IEnumerator AttackLogic()
    {
        yield return new WaitForSeconds(0.5f);
        var target = GetPreferTarget();
        ShootBullet(target);

        float atkTime = GetAnimTime("battle_attack");
        yield return PlayAnimAndWait("battle_attack", false, atkTime / GetStatus(StatusType.ATK_TIMESCALE));

        yield return new WaitForSeconds(0.1f);
    }
}
