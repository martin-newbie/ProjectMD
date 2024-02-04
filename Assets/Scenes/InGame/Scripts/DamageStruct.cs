using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageStruct
{
    public Dictionary<StatusType, float> defaultValues;
    public Dictionary<StatusType, float> increaseValues;

    public DamageStruct(Dictionary<StatusType, float> value)
    {
        defaultValues = value;
        increaseValues = new Dictionary<StatusType, float>();
    }

    public void SetValue(StatusType type, float value)
    {
        if (!defaultValues.ContainsKey(type))
        {
            defaultValues.Add(type, value);
        }
        else
        {
            defaultValues[type] = value;
        }
    }

    public void AddDefaultValue(StatusType type, float value)
    {
        if (!defaultValues.ContainsKey(type))
        {
            defaultValues.Add(type, value);
        }
        else
        {
            defaultValues[type] += value;
        }
    }

    public void AddIncreaseValue(StatusType type, float value)
    {
        if (!increaseValues.ContainsKey(type))
        {
            increaseValues.Add(type, value);
        }
        else
        {
            increaseValues[type] += value;
        }
    }

    public float GetValue(StatusType type)
    {
        float defaultValue = GetDefaultValue(type);
        float increaseValue = GetIncreaseValue(type);

        float result = defaultValue * increaseValue;
        return result;
    }

    float GetDefaultValue(StatusType type)
    {
        if (defaultValues.ContainsKey(type))
        {
            return defaultValues[type];
        }
        else
        {
            return 0f;
        }
    }

    float GetIncreaseValue(StatusType type)
    {
        if (increaseValues.ContainsKey(type))
        {
            return 1 + increaseValues[type] * 0.01f;
        }
        else
        {
            return 1f;
        }
    }
}