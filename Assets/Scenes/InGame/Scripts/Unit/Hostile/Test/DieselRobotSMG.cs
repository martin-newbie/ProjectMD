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
        spriteModel = subject.spriteModel;
        spriteModel.sprite = ResourceManager.Instance.dieselRobotSmg;
        spriteModel.flipX = true;
        model.gameObject.SetActive(false);
    }

    protected override IEnumerator AttackLogic()
    {
        yield return CommonBurstFire(3);
    }

    public override Vector3 GetBoneWorldPos(string key)
    {
        if(key == "bullet_pos")
        {
            int dir = spriteModel.transform.eulerAngles.x == 0 ? 1 : -1;
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

        target.OnDamage(GetDamageStruct(), this);

        var randPos = new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f));
        var startPos = GetBoneWorldPos(key);
        var targetPos = target.GetBoneWorldPos("body") + randPos;

        float randHeight = Random.Range(-0.2f, 0.2f);
        startPos.y += randHeight;
        float targetX = targetPos.x;
        var resultPos = new Vector3(targetX, startPos.y);

        var bullet = Instantiate(probBullet, startPos, Quaternion.identity);
        bullet.GetComponent<SpriteRenderer>().sprite = InGameSpriteManager.Instance.lazerSprite;
        bullet.StartBulletEffect(startPos, targetPos, 25f, null, 2);
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

    public override void SetModelRotByDir(int dir)
    {
        if (dir == 1)
            spriteModel.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (dir == -1)
            spriteModel.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}
