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
    #endregion

    #region state

    public float range = 0f;
    public float moveSpeed = 0f;
    public BehaviourState state;
    public UnitGroupType group;

    #endregion

    public UnitBehaviour(UnitObject _subject)
    {
        subject = _subject;

        transform = subject.transform;
        gameObject = subject.gameObject;
        model = subject.model;
    }

    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return subject.StartCoroutine(routine);
    }
    public void StopCoroutine(Coroutine routine)
    {
        subject.StopCoroutine(routine);
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

            int moveDir = target.transform.position.x < transform.position.x ? -1 : 1;
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
        PlayAnimAndWait("battle_wait", true);
    }

    protected virtual IEnumerator AttackAim()
    {
        yield return PlayAnimAndWait("battle_aiming");
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

    public virtual void SetBehaviourState(BehaviourState _state)
    {
        state = _state;

        switch (state)
        {
            case BehaviourState.STANDBY:
                // just play idle animation
                break;
            case BehaviourState.RETREAT:
                // return to start pos
                break;
        }
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

}
