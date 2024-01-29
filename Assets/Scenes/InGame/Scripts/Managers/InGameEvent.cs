using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameEvent : MonoBehaviour
{
    public static InGameEvent Instance = null;
    public Dictionary<EventType, List<IEventPost>> events = new Dictionary<EventType, List<IEventPost>>();

    private void Awake()
    {
        Instance = this;
    }

    public static void Add(EventType type, IEventPost subject)
    {
        if (!Instance.events.ContainsKey(type))
        {
            Instance.events.Add(type, new List<IEventPost>());
        }

        Instance.events[type].Add(subject);
    }

    public static void Remove(EventType type, IEventPost subject)
    {
        if (!Instance.events.ContainsKey(type))
            return;
        if (!Instance.events[type].Contains(subject))
            return;

        Instance.events[type].Remove(subject);
    }

    public static void Post(EventType type, UnitBehaviour from, UnitBehaviour to)
    {
        if (!Instance.events.ContainsKey(type))
            return;

        var list = Instance.events[type];
        foreach (var item in list)
        {
            item.PostEvent(type, from, to);
        }
    }
}

public interface IEventPost
{
    void PostEvent(EventType type, UnitBehaviour from, UnitBehaviour to);
}

public enum EventType
{
    HIT_ATTACK,
    HIT_CRITICAL,
    MISS_ATTACK,
    IMMUNE_ATTACK,
}
