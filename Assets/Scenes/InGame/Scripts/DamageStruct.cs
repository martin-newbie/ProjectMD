using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageStruct
{
    public Dictionary<StatusType, float> damageValues;

    public DamageStruct(Dictionary<StatusType, float> value)
    {
        damageValues = value;
    }

    public float GetValue(StatusType type)
    {
        return damageValues[type];
    }
}