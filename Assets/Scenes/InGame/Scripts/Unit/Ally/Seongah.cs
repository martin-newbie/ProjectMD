using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seongah : ActiveSkillBehaviour
{


    SeongahSkillBullet skillBullet;

    public Seongah(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
    {
        skillBullet = InGamePrefabsManager.GetObject("SeongahSkillBullet").GetComponent<SeongahSkillBullet>();
    }

    protected override IEnumerator AttackLogic()
    {
        var target = GetNearestOpponent();
        ShootBullet(target);

        yield return PlayAnimAndWait("battle_attack");
        yield return PlayAnimAndWait("battle_bolt");
        yield break;
    }

    public override IEnumerator ActiveSkill(SkillData skillData)
    {
        var target = GetNearestOpponent();
        skillData.damageData.SetValue(StatusType.PENETRATE, skillData.collabseCount + 1);
        PlayAnim("battle_wait", true);

        yield return PlayAnimAndWait("active_skill");
        var bullet = Instantiate(skillBullet, GetBoneWorldPos("bullet_pos"), model.transform.rotation);
        bullet.InitBulletAndShoot(this, skillData.damageData, GetOpponentGroup(), GetTargetDir(target));
        yield return PlayAnimAndWait("battle_attack");
    }

    public override void CollabseBuff(SkillData skillData, UnitBehaviour subjectUnit)
    {

    }

    public override bool ActiveSkillCondition()
    {
        bool isSkill = state == BehaviourState.ACTIVE_SKILL;
        bool isAlive = state != BehaviourState.RETIRE;
        bool emenyExists = InGameManager.Instance.allUnits.FindAll((item) => item.group == GetOpponentGroup()).Count > 0;
        return !isSkill && emenyExists && isAlive;
    }
}
