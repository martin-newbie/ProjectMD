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
    public int keyIndex = 0;
    public float range = 0f;
    public float moveSpeed = 3f;
    public float atkEndDelay = 1f;
    public float damage = 10f;
    public float hp = 100f;
    public float maxHp = 100f;
    public BehaviourState state;
    public UnitGroupType group;
    #endregion

    #region runningValue
    public int curAmmo;
    public int maxAmmo;
    public bool isAction;
    #endregion

    public Vector3 targetPos = new Vector3();
    public Coroutine actionCoroutine;

    public HpBarBase hpBar;

    public UnitBehaviour(UnitObject _subject)
    {
        subject = _subject;

        transform = subject.transform;
        gameObject = subject.gameObject;
        model = subject.model;

        probBullet = subject.probBullet;
    }

    public void InitCommon(int idx, int barType)
    {
        keyIndex = idx;

        hpBar = HpBarCanvas.Instance.GetHpBar(barType);
        hpBar.InitBar(maxHp);

        // set datas
        maxAmmo = StaticDataManager.GetConstUnitData(keyIndex).ammoCount;
        curAmmo = maxAmmo;
        range = StaticDataManager.GetConstUnitData(keyIndex).range;
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

        hpBar?.FollowPos(transform.position);
    }

    protected virtual void InCombatFunc()
    {

        var target = GetNearestOpponent();

        if (target == null)
        {
            isAction = false;
            return;
        }

        if (IsInsideRange(target))
        {
            // attack
            if (curAmmo <= 0)
            {
                StartActionCoroutine(Reload());
            }
            else
            {
                StartActionCoroutine(AttackToTarget());
            }
        }
        else
        {
            // move
            StartActionCoroutine(MoveToTargetRange());
        }
    }

    protected virtual void StartActionCoroutine(IEnumerator routine)
    {
        isAction = true;

        if (actionCoroutine != null) StopCoroutine(actionCoroutine);
        actionCoroutine = StartCoroutine(action());

        IEnumerator action()
        {
            state = BehaviourState.NOTHING;
            yield return StartCoroutine(routine);
            state = BehaviourState.INCOMBAT;
        }
    }

    #region Move
    public virtual Coroutine CommonMoveToPosition(Vector3 target)
    {
        if(target == transform.position)
        {
            return null;
        }

        if (target.x > transform.position.x)
        {
            SetModelRotByDir(1);
        }
        if (target.x < transform.position.x)
        {
            SetModelRotByDir(-1);
        }

        return StartCoroutine(moveLogic());

        IEnumerator moveLogic()
        {
            yield return StartCoroutine(MoveToTargetLerp(target));
            SetModelRotByDir(1);
        }
    }

    protected virtual IEnumerator MoveToTargetRange()
    {
        PlayAnim("battle_move", true);
        yield return StartCoroutine(CombatMoveLogic());
        PlayAnim("battle_wait", true);
    }

    protected virtual IEnumerator CombatMoveLogic()
    {
        while (true)
        {
            var target = GetNearestOpponent();
            targetPos = InGameManager.Instance.GetPreferPos(this, target, range);

            if (transform.position == targetPos)
            {
                break;
            }

            SetModelRotByTarget(target);
            var nextPos = InGameManager.Instance.GetNextPos(this, target, range);
            yield return StartCoroutine(MoveToTargetLerp(nextPos));
        }
    }

    protected virtual IEnumerator MoveToTargetLerp(Vector3 target)
    {
        float dur = Vector3.Distance(transform.position, target) / moveSpeed;
        float timer = 0f;
        Vector3 startPos = transform.position;

        while (timer <= dur)
        {
            transform.position = Vector3.Lerp(startPos, target, timer / dur);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = target;
        yield break;
    }
    #endregion

    #region Attack
    protected virtual IEnumerator AttackToTarget()
    {
        yield return StartCoroutine(AttackAim());
        yield return StartCoroutine(AttackLogic());
        yield return StartCoroutine(AttackFinish());
    }

    protected virtual IEnumerator Reload()
    {
        yield return PlayAnimAndWait("battle_reload");
        curAmmo = maxAmmo;
    }

    protected virtual IEnumerator AttackAim()
    {
        var target = GetNearestOpponent();
        SetModelRotByTarget(target);
        yield return PlayAnimAndWait("battle_aiming");
    }

    protected virtual IEnumerator AttackFinish()
    {
        PlayAnim("battle_wait", true);
        yield return new WaitForSeconds(atkEndDelay);
    }

    protected virtual IEnumerator CommonBurstFire(int count, float delay = 0.15f)
    {
        for (int i = 0; i < count; i++)
        {
            var target = GetNearestOpponent();
            ShootBullet(target);
            PlayAnim("battle_attack");
            yield return new WaitForSeconds(delay);
        }
    }

    protected abstract IEnumerator AttackLogic();
    #endregion

    protected virtual bool IsInsideRange(UnitBehaviour target)
    {
        return Vector3.Distance(transform.position, target.transform.position) <= range;
    }

    protected virtual UnitBehaviour GetNearestOpponent()
    {
        return InGameManager.Instance.FindNearestTarget(GetOpponentGroup(), transform.position);
    }

    protected virtual void ShootBullet(UnitBehaviour target, string key = "bullet_pos")
    {
        if (target == null)
        {
            return;
        }

        var randPos = new Vector3(0, Random.Range(-0.25f, 0.25f));
        var startPos = GetBoneWorldPos(key) + randPos;
        var targetPos = target.GetBoneWorldPos("body") + randPos;

        var bullet = Instantiate(probBullet, startPos, Quaternion.identity);
        bullet.StartBulletEffect(startPos, targetPos, 25f, () => target?.OnDamage(damage, this));
        curAmmo--;
    }

    public virtual void OnDamage(float damage, UnitBehaviour from)
    {
        // check death

        // calculate damage
        // give damage
        // print damage text

        int dir = transform.position.x < from.transform.position.x ? -1 : 1;
        DamageTextCanvas.Instance.PrintDamageText(damage, GetBoneWorldPos("body"), dir, ResistType.NORMAL);

        hp -= damage;
        hpBar.UpdateFill(hp);
        if (hp <= 0 && state != BehaviourState.RETIRE)
        {
            OnRetire();
        }
    }

    protected virtual void OnRetire()
    {
        subject.StopAllCoroutines();
        state = BehaviourState.RETIRE;
        InGameManager.Instance.RetireCharacter(this);
        PlayAnim("battle_retire");

        hpBar.DestroyBar();
        hpBar = null;
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
                PlayAnim("battle_move", true);
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

    public void SetModelRotByDir(int dir)
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

    protected TrackEntry AddAnim(string key, bool loop = false, float delay = 0f)
    {
        var track = model.state.AddAnimation(0, key, loop, delay);
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
