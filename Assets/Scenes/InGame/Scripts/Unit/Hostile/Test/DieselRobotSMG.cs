using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieselRobotSMG : UnitBehaviour
{

    SpriteRenderer spriteModel;

    public DieselRobotSMG(UnitData _unitData, Dictionary<StatusType, float> _statusData) : base(_unitData, _statusData)
    {
    }

    public override void InitObject(UnitObject _subject, int barType)
    {
        base.InitObject(_subject, barType);
        spriteModel = model.GetComponent<SpriteRenderer>();
        spriteModel.sprite = ResourceManager.Instance.dieselRobotSmg;
        spriteModel.flipX = true;
        model.enabled = false;
    }

    protected override IEnumerator AttackLogic()
    {
        yield return CommonBurstFire(3);
    }

    protected override void OnRetire()
    {
        base.OnRetire();

    }

    public override Vector3 GetBoneWorldPos(string key)
    {
        if(key == "bullet_pos")
        {
            int dir = model.transform.eulerAngles.x == 0 ? 1 : -1;
            return transform.position + new Vector3(dir, 0.9f);
        }
        if(key == "body")
        {
            return transform.position + new Vector3(0, 1);
        }

        return Vector3.zero;
    }

    protected override void ShootBullet(UnitBehaviour target, string key = "bullet_pos")
    {

        if (target == null)
        {
            return;
        }

        var randPos = new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f));
        var startPos = GetBoneWorldPos(key);
        var targetPos = target.GetBoneWorldPos("body") + randPos;

        var bullet = Instantiate(probBullet, startPos, Quaternion.identity);
        bullet.StartBulletEffect(startPos, targetPos, 25f, () => target?.OnDamage(GetDamageStruct(), this), 2);
        curAmmo--;
    }

    public override WaitForSeconds PlayAnimAndWait(string key, bool loop = false, float duration = -1)
    {
        if (duration <= 0) duration = 1;
        return new WaitForSeconds(duration);
    }

    protected override float GetAnimTime(string key)
    {
        return 0f;
    }
}
