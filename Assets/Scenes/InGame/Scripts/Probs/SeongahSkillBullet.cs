using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeongahSkillBullet : MonoBehaviour
{

    [SerializeField] float bulletSpeed;
    [SerializeField] ParticleSystem bulletTail;

    UnitBehaviour owner;
    UnitGroupType targetType;
    DamageStruct damage;
    int dir;

    PenetrateEffect penetrateEffect;

    bool moveable = false;

    public void InitBulletAndShoot(UnitBehaviour _owner, DamageStruct _damage, UnitGroupType _targetType, int _dir /* 1 | -1 */)
    {
        owner = _owner;
        targetType = _targetType;
        damage = _damage;
        dir = _dir;

        int rotY = dir == 1 ? 0 : -180;
        transform.rotation = Quaternion.Euler(0, rotY, 0);

        penetrateEffect = InGamePrefabsManager.GetObject("SeongahPenetrate").GetComponent<PenetrateEffect>();
        moveable = true;
    }

    private void Update()
    {
        if (!moveable) return;

        transform.Translate(Vector3.right * Time.deltaTime * bulletSpeed * dir);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!moveable) return;
        if (!collision.CompareTag("Player")) return;

        var target = collision.GetComponent<UnitObject>().behaviour;
        if (target.group != targetType) return;
        if (target.state == BehaviourState.RETIRE) return;

        target.OnDamage(damage, owner);
        var effect = Instantiate(penetrateEffect, new Vector3(target.transform.position.x, transform.position.y, 0), transform.rotation);
        effect.StartEffect();

        damage.SetValue(StatusType.PENETRATE, damage.GetValue(StatusType.PENETRATE) - 1);
        
        if(damage.GetValue(StatusType.PENETRATE) <= 0)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            bulletTail.Stop();
            moveable = false;
            Destroy(gameObject, 1f);
            return;
        }
    }
}
