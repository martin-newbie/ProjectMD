using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity
{
    public UnitObject subject;
    public Transform transform;
    public GameObject gameObject;
    public SkeletonAnimation model;
    public ParticleSystem hpEffect;
    public Bullet probBullet;

    public float hp;
    public ConstUnitData constData;
    public bool isAction;

    public bool isCC;
    public Coroutine ccRoutine;
    public Entity tauntTarget;

    public UnitGroupType group;
    public Dictionary<StatusType, float> statusData;
    public Dictionary<StatusType, float> buffList;
    public Dictionary<StatusType, float> debuffList;
    public List<Timer> buffTimers;
    public List<Timer> debuffTimers;
    public List<IKeyword> activeKeywords;
    public List<Coroutine> activatingRoutines = new List<Coroutine>();
    public BehaviourState state;

    public virtual void OnHeal(float value, Entity from)
    {
        if (state == BehaviourState.STANDBY || state == BehaviourState.RETIRE) return;
        GetHeal(value);
    }
    protected virtual void GetHeal(float value)
    {
        hp += value * GetStatus(StatusType.HEAL_RAISE);
    }
    public virtual void OnDamage(DamageStruct value, Entity from)
    {
        if (state == BehaviourState.STANDBY || state == BehaviourState.RETIRE) return;

        float damage = value.GetValue(StatusType.DMG);
        // check death

        float affinityModify;
        bool isCri = false;

        ResistType resist;
        affinityModify = from.constData.damageToRace[constData.raceType];
        if (affinityModify > 1f)
        {
            resist = ResistType.WEAK;
        }
        else if (affinityModify == 1f)
        {
            resist = ResistType.NORMAL;
        }
        else
        {
            resist = ResistType.RESIST;
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

            activeKeywords.ForEach(item => { if (item is IKeywordOnDamage) (item as IKeywordOnDamage).OnDamage(value, from, this); });
            GetDamage(damage);
        }

        int dir = transform.position.x < from.transform.position.x ? -1 : 1;
        DamageTextCanvas.Instance.PrintDamageText(damage, GetBoneWorldPos("body"), dir, resist, isCri);


        float randomRate()
        {
            return Random.Range(0f, 1f);
        }
    }
    public virtual void OnDamageForce(float damage, Entity from)
    {
        if (state == BehaviourState.STANDBY || state == BehaviourState.RETIRE) return;

        ResistType resist = ResistType.NORMAL;
        float def = (1666f / (1666f + GetStatus(StatusType.DEF)));
        damage *= def;
        int dir = transform.position.x < from.transform.position.x ? -1 : 1;
        DamageTextCanvas.Instance.PrintDamageText(damage, GetBoneWorldPos("body"), dir, resist, false);
        GetDamage(damage);
    }
    protected virtual void GetDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            OnRetire();
        }
    }
    protected abstract void OnRetire();
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
    public virtual void AddKeyword(KeywordType type, int count)
    {
        IKeyword keyword = null;
        switch (type)
        {
            case KeywordType.TREMOR:
                keyword = new Tremor(count);
                break;
            case KeywordType.SHOCK:
                break;
            case KeywordType.BLEED:
                break;
            case KeywordType.BURN:
                break;
        }
    }
    public virtual void GetTaunted(Entity from, float time)
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

    public virtual Vector3 GetBoneWorldPos(string key)
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
        Dictionary<StatusType, float> structValue = new Dictionary<StatusType, float>
        {
            { StatusType.DMG, GetStatus(StatusType.DMG) },
            { StatusType.ACCURACY, GetStatus(StatusType.ACCURACY) },
            { StatusType.CRI_RATE, GetStatus(StatusType.CRI_RATE) },
            { StatusType.CRI_DAMAGE, GetStatus(StatusType.CRI_DAMAGE) },
            { StatusType.STABLE, GetStatus(StatusType.STABLE) }
        };

        DamageStruct dmg = new DamageStruct(structValue);
        return dmg;
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
}
