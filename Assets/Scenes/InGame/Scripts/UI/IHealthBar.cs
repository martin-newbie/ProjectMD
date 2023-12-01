using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthBar
{
    float hp { get; set; }
    float maxHP { get; set; }

    void InitBar(float _maxHp);
    void FollowPos(Vector3 pos);
    void SetBar(float _hp);
    void DestroyBar();
}
