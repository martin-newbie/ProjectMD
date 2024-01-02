using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public abstract class UnitBehaviour
{
    #region component
    public UnitObject subject;
    public Transform transform;
    public GameObject gameObject;
    public SkeletonAnimation model;

    public Bullet probBullet;
    #endregion

    public UnitStatus staticStatus;
    public ConstUnitData constData;

    #region state
    public float hp;
    public int level;
    public int keyIndex;
    #endregion

    #region runningValue
    public int curAmmo;
    public int maxAmmo;
    public bool isAction;
    public BehaviourState state;
    public UnitGroupType group;
    public Dictionary<StatusType, float> buffList;
    public Dictionary<StatusType, float> debuffList;
    #endregion

    public Vector3 targetPos = new Vector3();
    public Coroutine actionCoroutine;
    public HpBarBase hpBar;

    Action retireAction;

    public UnitBehaviour(UnitObject _subject)
    {
        subject = _subject;

        transform = subject.transform;
        gameObject = subject.gameObject;
        model = subject.model;

        probBullet = subject.probBullet;

        buffList = new Dictionary<StatusType, float>();
        debuffList = new Dictionary<StatusType, float>();
    }

    public virtual void InitCommon(int idx, int barType)
    {
        keyIndex = idx;
        staticStatus = StaticDataManager.GetUnitStatus(keyIndex);
        constData = StaticDataManager.GetConstUnitData(keyIndex);

        hp = GetStatus(StatusType.HP);
        hpBar = HpBarCanvas.Instance.GetHpBar(barType);
        hpBar.InitBar(GetStatus(StatusType.HP));

        // set datas
        maxAmmo = constData.ammoCount;
        curAmmo = maxAmmo;
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
    public virtual IEnumerator CommonMoveToPosEndWait(Vector3 target)
    {
        if (target == transform.position)
        {
            yield break;
        }

        PlayAnim("battle_move", true);
        if (target.x > transform.position.x)
        {
            SetModelRotByDir(1);
        }
        if (target.x < transform.position.x)
        {
            SetModelRotByDir(-1);
        }

        StartActionCoroutine(MoveToEnd(target));
        yield return actionCoroutine;

        PlayAnim("battle_wait", true);
        SetModelRotByDir((int)group);
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
            if (IsInsideRange(target))
            {
                break;
            }

            MoveToTarget(target);
            yield return null;
        }
    }

    protected virtual IEnumerator MoveToEnd(Vector3 endPos)
    {
        float dur = Vector3.Distance(endPos, transform.position) / GetStatus(StatusType.MOVE_SPEED);
        float timer = 0f;
        var startPos = transform.position;

        while (timer < dur)
        {
            transform.position = Vector3.Lerp(startPos, endPos, timer / dur);
            timer += Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    protected virtual void MoveToTarget(UnitBehaviour target)
    {
        SetModelRotByTarget(target);
        int dir = GetTargetDir(target);
        transform.Translate(Vector3.right * dir * GetStatus(StatusType.MOVE_SPEED) * Time.deltaTime);
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
        yield return new WaitForSeconds(GetStatus(StatusType.ATK_DELAY));
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
        return Vector3.Distance(transform.position, target.transform.position) <= GetStatus(StatusType.RANGE);
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
        bullet.StartBulletEffect(startPos, targetPos, 25f, () => target?.OnDamage(GetDamageStruct(), this));
        curAmmo--;
    }
    public virtual void InjectDeadEvent(Action _retireAction)
    {
        retireAction = _retireAction;
    }
    public virtual void OnDamage(DamageStruct value, UnitBehaviour from)
    {

        float damage = value.GetValue(StatusType.DMG);
        // check death

        int atk = from.constData.atkType;
        int def = constData.defType;
        ResistType resist;
        if (atk == def)
        {
            resist = ResistType.WEAK;
        }
        else if ((atk == 0 && def == 1) || (atk == 1 && def == 2) || (atk == 2 && def == 0))
        {
            resist = ResistType.NORMAL;
        }
        else
        {
            resist = ResistType.RESIST;
        }

        // calculate damage
        // give damage
        // print damage text

        int dir = transform.position.x < from.transform.position.x ? -1 : 1;
        DamageTextCanvas.Instance.PrintDamageText(damage, GetBoneWorldPos("body"), dir, resist);

        hp -= damage;
        hpBar?.UpdateFill(hp);
        if (hp <= 0 && state != BehaviourState.RETIRE)
        {
            OnRetire();
        }
    }
    protected virtual void OnRetire()
    {
        subject.StopAllCoroutines();
        state = BehaviourState.RETIRE;
        PlayAnim("battle_retire");
        retireAction?.Invoke();
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
    public float GetStatus(StatusType type)
    {
        float result = 0;
        result += staticStatus.GetTotalStatus(type, level);

        // add result from equipment
        // add result from unique weapon

        // add result from buff and debuff
        #region buff/debuff
        float buff = 0f;
        float debuff = 0f;

        if (buffList.ContainsKey(type) && buffList[type] > 0f)
        {
            buff = buffList[type];
        }
        if (debuffList.ContainsKey(type) && debuffList[type] > 0f)
        {
            debuff = debuffList[type];
            if (debuff > 0.8f) debuff = 0.8f; // maximum debuff value
        }

        float buffedValue = result * buff;
        float debuffedValue = result * debuff;

        result += buffedValue;
        result -= debuffedValue;
        #endregion

        return result;
    }
    public DamageStruct GetDamageStruct()
    {
        Dictionary<StatusType, float> structValue = new Dictionary<StatusType, float>();
        structValue.Add(StatusType.DMG, GetStatus(StatusType.DMG));
        structValue.Add(StatusType.ACCURACY, GetStatus(StatusType.ACCURACY));
        structValue.Add(StatusType.CRI_RATE, GetStatus(StatusType.CRI_RATE));
        structValue.Add(StatusType.CRI_DAMAGE, GetStatus(StatusType.CRI_DAMAGE));
        structValue.Add(StatusType.STABLE, GetStatus(StatusType.STABLE));

        DamageStruct dmg = new DamageStruct(structValue);
        return dmg;
    }
    public void ActiveUnit()
    {
        if (state == BehaviourState.RETIRE)
        {
            return;
        }

        state = BehaviourState.STANDBY;
        gameObject.SetActive(true);
        hpBar.gameObject.SetActive(true);
    }
    public void DeactiveUnit()
    {
        hpBar.gameObject.SetActive(false);
    }
}
