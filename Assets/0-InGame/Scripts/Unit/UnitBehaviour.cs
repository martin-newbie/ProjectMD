using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBehaviour
{
    #region component
    public UnitObject subject;
    public Transform transform;
    public GameObject gameObject;
    public SkeletonAnimation model;

    public Bullet probBullet;
    #endregion

    #region state
    public float range = 0f;
    public float moveSpeed = 3f;
    public float atkEndDelay = 1f;
    public float damage = 10f;
    public float hp = 100f;
    public float maxHp = 100f;
    public BehaviourState state;
    public UnitGroupType group;
    #endregion

    public UnitBehaviour(UnitObject _subject)
    {
        subject = _subject;

        transform = subject.transform;
        gameObject = subject.gameObject;
        model = subject.model;

        probBullet = subject.probBullet;
    }

    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return subject.StartCoroutine(routine);
    }
    public void StopCoroutine(Coroutine routine)
    {
        subject.StopCoroutine(routine);
    }
    public static T Instantiate<T>(T original, Vector3 position, Quaternion rotation) where T : Object
    {
        return Object.Instantiate(original, position, rotation);
    }


    public virtual void Update()
    {
        switch (state)
        {
            case BehaviourState.INCOMBAT:
                InCombatFunc();
                break;
        }
    }

    protected virtual void InCombatFunc()
    {
        var target = GetOpponent();

        if (target == null) return;

        if (IsInsideRange(target))
        {
            // attack
            StartActionCoroutine(AttackToTarget());
        }
        else
        {
            // move
            StartActionCoroutine(MoveToTargetRange());
        }
    }

    protected virtual void StartActionCoroutine(IEnumerator routine)
    {
        StartCoroutine(action());

        IEnumerator action()
        {
            state = BehaviourState.NOTHING;
            yield return StartCoroutine(routine);
            state = BehaviourState.INCOMBAT;
        }
    }

    #region Move
    protected virtual IEnumerator MoveToTargetRange()
    {
        PlayAnim("battle_move", true);
        yield return StartCoroutine(MoveLogic());
        PlayAnim("battle_wait", true);
    }

    protected virtual IEnumerator MoveLogic()
    {
        while (true)
        {
            var target = GetOpponent();
            if (IsInsideRange(target))
            {
                break;
            }

            int moveDir = GetTargetDir(target);
            transform.Translate(moveDir * Vector3.right * Time.deltaTime * moveSpeed);
            SetModelRotByDir(moveDir);
            yield return null;
        }
    }
    #endregion

    #region Attack
    protected virtual IEnumerator AttackToTarget()
    {
        yield return StartCoroutine(AttackAim());
        yield return StartCoroutine(AttackLogic());
        yield return StartCoroutine(AttackFinish());
    }

    protected virtual IEnumerator AttackAim()
    {
        var target = GetOpponent();
        SetModelRotByTarget(target);
        yield return PlayAnimAndWait("battle_aiming");
    }

    protected virtual IEnumerator AttackFinish()
    {
        PlayAnim("battle_wait", true);
        yield return new WaitForSeconds(atkEndDelay);
    }

    protected abstract IEnumerator AttackLogic();
    #endregion

    protected virtual bool IsInsideRange(UnitBehaviour target)
    {
        return transform.position.x > target.transform.position.x - range && transform.position.x < target.transform.position.x + range;
    }

    protected virtual UnitBehaviour GetOpponent()
    {
        return InGameManager.Instance.FindNearestTarget(GetOpponentGroup(), transform.position.x);
    }

    protected virtual void ShootBullet(UnitBehaviour target, string key = "bullet_pos")
    {
        if(target == null)
        {
            return;
        }

        var randPos = new Vector3(0, Random.Range(-0.25f, 0.25f));
        var startPos = GetBoneWorldPos(key) + randPos;
        var targetPos = target.GetBoneWorldPos("body") + randPos;

        var bullet = Instantiate(probBullet, startPos, Quaternion.identity);
        bullet.StartBulletEffect(startPos, targetPos, () => target.OnDamage(damage, this));
    }

    protected virtual void OnDamage(float damage, UnitBehaviour from)
    {
        // check death

        // calculate damage
        // give damage
        // print damage text

        int dir = transform.position.x < from.transform.position.x ? -1 : 1;
        DamageTextCanvas.Instance.PrintDamageText(damage, GetBoneWorldPos("body"), dir, ResistType.NORMAL);

        hp -= damage;
        if(hp <= 0 && state != BehaviourState.RETIRE)
        {
            OnRetire();
        }
    }

    protected virtual void OnRetire()
    {
        subject.StopAllCoroutines();
        state = BehaviourState.RETIRE;
        InGameManager.Instance.allUnits.Remove(this.subject);
        PlayAnim("battle_retire");
    }

    public virtual void SetBehaviourState(BehaviourState _state)
    {
        state = _state;

        switch (state)
        {
            case BehaviourState.STANDBY:
                // just play idle animation
                PlayAnim("battle_wait", true);
                break;
            case BehaviourState.RETREAT:
                // return to start pos
                break;
        }
    }

    protected void SetModelRotByTarget(UnitBehaviour target)
    {
        int dir = GetTargetDir(target);
        SetModelRotByDir(dir);
    }

    protected int GetTargetDir(UnitBehaviour target)
    {
        return target.transform.position.x < transform.position.x ? -1 : 1;
    }

    protected void SetModelRotByDir(int dir)
    {
        if (dir == 1)
            model.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (dir == -1)
            model.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    protected WaitForSeconds PlayAnimAndWait(string key, bool loop = false)
    {
        PlayAnim(key, loop);
        var anim = model.Skeleton.Data.FindAnimation(key);
        return new WaitForSeconds(anim.Duration);
    }

    protected TrackEntry PlayAnim(string key, bool loop = false)
    {
        var track = model.state.SetAnimation(0, key, loop);
        return track;
    }

    protected UnitGroupType GetOpponentGroup()
    {
        return (UnitGroupType)((int)group * -1);
    }

    protected Vector3 GetBoneWorldPos(string key)
    {
        var bone = model.skeleton.FindSlot(key).Bone;
        Vector3 result = model.transform.TransformPoint(new Vector3(bone.WorldX, bone.WorldY, 0f));

        return result;
    }
}
