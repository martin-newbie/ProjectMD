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

    public UnitData unitData;
    public UnitStatus staticStatus;
    public ConstUnitData constData;

    protected bool nowActive = false;

    #region runningValue
    public float hp;
    public int curAmmo;
    public int maxAmmo;
    public bool isAction;
    public BehaviourState state;
    public UnitGroupType group;
    public Dictionary<StatusType, float> statusData;
    public Dictionary<StatusType, float> buffList;
    public Dictionary<StatusType, float> debuffList;
    public List<Timer> buffTimers;
    public List<Timer> debuffTimers;
    #endregion

    #region CC
    public bool isCC;
    public Coroutine ccRoutine;
    public UnitBehaviour tauntTarget;
    #endregion

    public Vector3 targetPos = new Vector3();
    protected List<Coroutine> activatingRoutines = new List<Coroutine>();
    public HpBarBase hpBar;

    Action retireAction;

    public UnitBehaviour(UnitData _unitData, Dictionary<StatusType, float> _statusData)
    {
        unitData = _unitData;
        statusData = _statusData;
    }

    public virtual void InitObject(UnitObject _subject, int barType)
    {
        subject = _subject;

        transform = subject.transform;
        gameObject = subject.gameObject;
        model = subject.model;

        probBullet = subject.probBullet;

        buffList = new Dictionary<StatusType, float>();
        debuffList = new Dictionary<StatusType, float>();
        buffTimers = new List<Timer>();
        debuffTimers = new List<Timer>();

        constData = StaticDataManager.GetConstUnitData(unitData.index);

        hp = GetStatus(StatusType.HP);
        hpBar = HpBarCanvas.Instance.GetHpBar(barType);
        hpBar.InitBar(GetStatus(StatusType.HP));
    }
    public virtual void UnitActive()
    {
        nowActive = true;

        SetActiveHpBar(true);

        maxAmmo = constData.ammoCount;
        curAmmo = maxAmmo;
    }
    public Coroutine StartCoroutine(IEnumerator function, bool isAction = true)
    {
        var coroutine = subject.StartCoroutine(function);
        if (isAction)
        {
            activatingRoutines.Add(coroutine);
        }
        return coroutine;
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
        if (!nowActive) return;

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

        var target = GetPreferTarget();

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

    public virtual Coroutine StartActionCoroutine(IEnumerator routine)
    {
        isAction = true;

        StopAllCoroutine();
        return StartCoroutine(action());

        IEnumerator action()
        {
            state = BehaviourState.ACTING;
            yield return StartCoroutine(routine);
            state = BehaviourState.INCOMBAT;
        }
    }

    protected void StopAllCoroutine()
    {
        foreach (var coroutine in activatingRoutines)
        {
            if (coroutine != null) StopCoroutine(coroutine);
        }

        activatingRoutines = new List<Coroutine>();
    }

    #region Move
    public virtual IEnumerator CommonMoveToPosEndWait(Vector3 target)
    {
        if (target == transform.position)
        {
            yield break;
        }

        PlayAnimAndWait("battle_move", true);
        if (target.x > transform.position.x)
        {
            SetModelRotByDir(1);
        }
        if (target.x < transform.position.x)
        {
            SetModelRotByDir(-1);
        }

        yield return StartActionCoroutine(MoveToEnd(target));

        PlayAnimAndWait("battle_wait", true);
        SetModelRotByDir((int)group);
    }

    protected virtual IEnumerator MoveToTargetRange()
    {
        PlayAnimAndWait("battle_move", true);
        yield return StartCoroutine(CombatMoveLogic());
        PlayAnimAndWait("battle_wait", true);
    }

    protected virtual IEnumerator CombatMoveLogic()
    {
        while (true)
        {
            var target = GetPreferTarget();
            if (target == null)
            {
                break;
            }
            if (IsInsideRange(target))
            {
                break;
            }

            MoveToTargetFrame(target);
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

    protected virtual void MoveToTargetFrame(UnitBehaviour target)
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
        // add reload time
        float reloadTime = GetAnimTime("battle_reload");
        yield return PlayAnimAndWait("battle_reload", false, reloadTime / GetStatus(StatusType.ATK_TIMESCALE));
        curAmmo = maxAmmo;
    }

    protected virtual IEnumerator AttackAim()
    {
        var target = GetPreferTarget();
        SetModelRotByTarget(target);

        float aimingTime = GetAnimTime("battle_aiming");
        yield return PlayAnimAndWait("battle_aiming", false, aimingTime / GetStatus(StatusType.ATK_TIMESCALE));
    }

    protected virtual IEnumerator AttackFinish()
    {
        PlayAnimAndWait("battle_wait", true);
        yield return new WaitForSeconds(GetStatus(StatusType.ATK_DELAY) / GetStatus(StatusType.ATK_TIMESCALE));
    }

    protected virtual IEnumerator CommonBurstFire(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var target = GetPreferTarget();
            ShootBullet(target);
            float delay = GetStatus(StatusType.RPM);
            yield return PlayAnimAndWait("battle_attack", false, delay / GetStatus(StatusType.ATK_TIMESCALE));
        }
    }

    protected abstract IEnumerator AttackLogic();
    #endregion

    public virtual void GetTaunted(UnitBehaviour from, float time)
    {
        // after resist
        ccRoutine = StartCoroutine(taunt(), false);
        IEnumerator taunt()
        {
            isCC = true;
            tauntTarget = from;
            yield return new WaitForSeconds(time);
            isCC = false;
            tauntTarget = null;
        }
    }
    protected virtual bool IsInsideRange(UnitBehaviour target)
    {
        return Vector3.Distance(transform.position, target.transform.position) <= GetStatus(StatusType.RANGE);
    }
    protected virtual UnitBehaviour GetPreferTarget()
    {
        if (isCC && tauntTarget != null)
        {
            return tauntTarget;
        }
        else
        {
            return FindNearestTarget();
        }
    }
    protected virtual UnitBehaviour FindNearestTarget()
    {
        return InGameManager.Instance.FindNearestTarget(GetOpponentGroup(), transform.position);
    }
    protected virtual void ShootBullet(UnitBehaviour target, string key = "bullet_pos")
    {
        if (target == null)
        {
            return;
        }

        var randPos = new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f));
        var startPos = GetBoneWorldPos(key);
        var targetPos = target.GetBoneWorldPos("body") + randPos;

        var bullet = Instantiate(probBullet, startPos, Quaternion.identity);
        bullet.StartBulletEffect(startPos, targetPos, 25f, () => target?.OnDamage(GetDamageStruct(), this));
        curAmmo--;
    }
    public virtual void InjectDeadEvent(Action _retireAction)
    {
        retireAction = _retireAction;
    }
    public virtual void AddBuff(StatusType type, float value, float time, string key = "buff")
    {
        if (!buffList.ContainsKey(type)) buffList.Add(type, 0f);
        buffList[type] += value;

        if (time > 0f)
        {
            StartCoroutine(buff(), false);
        }

        IEnumerator buff()
        {
            var timer = new Timer(time, key);
            buffTimers.Add(timer);
            while (timer.t < 1)
            {
                timer.t += Time.deltaTime / timer.time;
                yield return null;
            }
            buffTimers.Remove(timer);
            buffList[type] -= value;
        }
    }
    public virtual void AddDebuff(StatusType type, float value, float time, string key = "debuff")
    {
        if (!debuffList.ContainsKey(type)) debuffList.Add(type, 0f);
        debuffList[type] += value;

        if (time > 0f)
        {
            StartCoroutine(debuff(), false);
        }

        IEnumerator debuff()
        {
            var timer = new Timer(time, key);
            debuffTimers.Add(timer);
            while (timer.t < 1)
            {
                timer.t += Time.deltaTime / timer.time;
                yield return null;
            }
            debuffTimers.Remove(timer);
            debuffList[type] -= value;
        }
    }
    public virtual void OnHeal(float value, UnitBehaviour from)
    {
        if (state == BehaviourState.STANDBY || state == BehaviourState.RETIRE) return;

        hp += value * GetStatus(StatusType.HEAL_RAISE);
    }
    public virtual void OnDamage(DamageStruct value, UnitBehaviour from)
    {
        if (state == BehaviourState.STANDBY || state == BehaviourState.RETIRE) return;

        float damage = value.GetValue(StatusType.DMG);
        // check death

        int atkType = from.constData.atkType;
        int defType = constData.defType;
        float affinityModify;
        bool isCri = false;

        ResistType resist;
        if (atkType == defType)
        {
            resist = ResistType.WEAK;
            affinityModify = 1.5f;
        }
        else if ((atkType == 0 && defType == 1) || (atkType == 1 && defType == 2) || (atkType == 2 && defType == 0))
        {
            resist = ResistType.NORMAL;
            affinityModify = 1f;
        }
        else
        {
            resist = ResistType.RESIST;
            affinityModify = 0.5f;
        }

        float hitRate = (700 / ((GetStatus(StatusType.DODGE) - value.GetValue(StatusType.ACCURACY)) + 700));
        if (hitRate < randomRate())
        {
            resist = ResistType.MISS;
            InGameEvent.Post(EventType.MISS_ATTACK, from, this);
        }
        else
        {
            InGameEvent.Post(EventType.HIT_ATTACK, from, this);

            // defense
            float def = (1666f / (1666f + GetStatus(StatusType.DEF)));

            // critical
            float criRate = value.GetValue(StatusType.CRI_RATE) * 0.01f;
            isCri = criRate > randomRate();

            damage *= def;
            damage *= affinityModify;
            damage *= isCri ? value.GetValue(StatusType.CRI_DAMAGE) * 0.01f : 1f;

            GetDamage(damage);
        }

        int dir = transform.position.x < from.transform.position.x ? -1 : 1;
        DamageTextCanvas.Instance.PrintDamageText(damage, GetBoneWorldPos("body"), dir, resist, isCri);


        float randomRate()
        {
            return Random.Range(0f, 1f);
        }
    }
    protected virtual void GetDamage(float damage)
    {
        hp -= damage;
        hpBar?.UpdateFill(hp);
        if (hp <= 0)
        {
            OnRetire();
        }
    }
    protected virtual void OnRetire()
    {
        subject.StopAllCoroutines();
        state = BehaviourState.RETIRE;
        PlayAnimAndWait("battle_retire");
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
                PlayAnimAndWait("battle_wait", true);
                break;
            case BehaviourState.RETREAT:
                PlayAnimAndWait("battle_move", true);
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
    public WaitForSeconds PlayAnimAndWait(string key, bool loop = false, float duration = -1f)
    {
        PlayAnimAndWait(key, loop);
        var animTime = GetAnimTime(key);
        if (duration <= 0f)
        {
            model.timeScale = 1f;
            return new WaitForSeconds(animTime);
        }
        else
        {
            float timeScale = animTime / duration;
            model.timeScale = timeScale;
            return new WaitForSeconds(duration);
        }
    }
    protected float GetAnimTime(string key)
    {
        return model.skeleton.Data.FindAnimation(key).Duration;
    }
    protected TrackEntry PlayAnim(string key, bool loop = false)
    {
        var track = model.state.SetAnimation(0, key, loop);
        return track;
    }
    public TrackEntry AddAnim(string key, bool loop = false, float delay = 0f)
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
        result += statusData[type];

        // add result from equipment
        // add result from unique weapon

        // add result from buff and debuff
        #region buff/debuff
        float buff = 0f;
        float debuff = 0f;

        if (buffList.ContainsKey(type) && buffList[type] > 0f)
        {
            buff = buffList[type] / 100f;
        }
        if (debuffList.ContainsKey(type) && debuffList[type] > 0f)
        {
            debuff = debuffList[type] / 100f;
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

    public void SetActiveHpBar(bool active)
    {
        hpBar.gameObject.SetActive(active);
    }
}
