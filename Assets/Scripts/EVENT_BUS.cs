using System;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    REQUEST_MUSIC,
    EVENT_STARTED,
    ATTACK,
    THROW_ITEM,
    ENEMY_KILLED,
    PLAYER_HURT,
    TOILET_UNCLOGGED,
    GHOSTS_BUSTED,
    ITEM_PICKUP,
    NUKE_DETECTED_PLAYER,
    NUKE_NOT_DETECTED_PLAYER,
    NUKE_DEFUSED,
    TIMER_FINISHED,
    TIMER_STARTED,
    TIMER_STOPPED,
    TASK_COMPLETED,
    LANTERN_PICKED_UP,
    LANTERN_DROPPED,
    BOB_FED,
    SPELL_CAST_SUCCESS,
}

public static class EVENT_BUS
{
    private static readonly Dictionary<EventType, Action<PublishEventArgs>> eventDictionary = new Dictionary<EventType, Action<PublishEventArgs>>();

    public static void Subscribe(EventType eventType, Action<PublishEventArgs> listener)
    {
        if (!eventDictionary.ContainsKey(eventType))
        {
            eventDictionary[eventType] = null;
        }

        eventDictionary[eventType] += listener;
    }

    public static void Unsubscribe(EventType eventType, Action<PublishEventArgs> listener)
    {
        if (eventDictionary.ContainsKey(eventType))
        {
            eventDictionary[eventType] -= listener;
        }
    }

    public static void Publish(EventType eventType, PublishEventArgs args)
    {
        if (eventDictionary.TryGetValue(eventType, out Action<PublishEventArgs> thisEvent))
        {
            thisEvent?.Invoke(args);
            //Debug.Log($"Event invoked: {eventType}");
        }
    }
}

public record PublishEventArgs(Dictionary<string, object> Data);

namespace System.Runtime.CompilerServices
{
    // Dummy class to enable modern C# record features on older frameworks
    internal static class IsExternalInit { }
}
