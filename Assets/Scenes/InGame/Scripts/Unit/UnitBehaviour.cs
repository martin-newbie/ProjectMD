using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class UnitBehaviour : Entity
{

    public UnitData unitData;
    public UnitStatus staticStatus;

    protected bool nowActive = false;

    public float hpAmount => hp / GetStatus(StatusType.HP);
    public int curAmmo;
    public int maxAmmo;


    public Vector3 targetPos = new Vector3();
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
        hpEffect = subject.hpRecoveryEffect;
        probBullet = subject.probBullet;

        buffList = new Dictionary<StatusType, float>();
        debuffList = new Dictionary<StatusType, float>();
        buffTimers = new List<Timer>();
        debuffTimers = new List<Timer>();
        activeKeywords = new List<IKeyword>();

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
    protected virtual void MoveToTargetFrame(Entity target)
    {
        SetModelRotByTarget(target);
        int dir = GetTargetDir(target);
        transform.Translate(Vector3.right * dir * GetStatus(StatusType.MOVE_SPEED) * Time.deltaTime);
    }
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

    protected virtual bool IsInsideRange(Entity target)
    {
        return Vector3.Distance(transform.position, target.transform.position) <= GetStatus(StatusType.RANGE);
    }
    protected virtual Entity GetPreferTarget()
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
    protected virtual Entity FindNearestTarget()
    {
        return InGameManager.Instance.FindNearestTarget(GetOpponentGroup(), transform.position);
    }
    protected virtual void ShootBullet(Entity target, string key = "bullet_pos")
    {
        if (target == null)
        {
            return;
        }

        var randPos = new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f));
        var startPos = GetBoneWorldPos(key);
        var targetPos = target.GetBoneWorldPos("body") + randPos;

        var bullet = Instantiate(probBullet, startPos, Quaternion.identity);
        bullet.ShootBullet(startPos, targetPos, 25f, () => target?.OnDamage(GetDamageStruct(), this));
        curAmmo--;
    }
    public virtual void InjectDeadEvent(Action _retireAction)
    {
        retireAction = _retireAction;
    }
    protected override void OnRetire()
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
    }
    protected void SetModelRotByTarget(Entity target)
    {
        int dir = GetTargetDir(target);
        SetModelRotByDir(dir);
    }
    protected int GetTargetDir(Entity target)
    {
        return target.transform.position.x < transform.position.x ? -1 : 1;
    }
    public virtual void SetModelRotByDir(int dir)
    {
        if (dir == 1)
            model.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (dir == -1)
            model.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
    public virtual WaitForSeconds PlayAnimAndWait(string key, bool loop = false, float duration = -1f)
    {
        PlayAnim(key, loop);
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
    protected virtual float GetAnimTime(string key)
    {
        return model.skeleton.Data.FindAnimation(key).Duration;
    }
    protected virtual TrackEntry PlayAnim(string key, bool loop = false)
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
    protected override void GetHeal(float value)
    {
        hpEffect.Play();
        base.GetHeal(value);
    }
    protected override void GetDamage(float damage)
    {
        hpBar?.UpdateFill(hp);
        base.GetDamage(damage);
    }
    public void SetActiveHpBar(bool active)
    {
        hpBar.gameObject.SetActive(active);
    }
    protected virtual void OnKeywordEvent()
    {

    }
}
