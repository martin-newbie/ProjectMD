using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HpBarBase : MonoBehaviour
{
    protected float hp;
    protected float maxHP;

    public abstract void InitBar(float _maxHp);
    public abstract void FollowPos(Vector3 pos);
    public abstract void UpdateFill(float _hp);
    public abstract void DestroyBar();
}
