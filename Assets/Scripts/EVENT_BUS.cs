using System;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    ATTACK,
    ENEMY_KILLED,
    PLAYER_HURT,
    TOILET_UNCLOGGED,
    GHOSTS_BUSTED,
    ITEM_PICKUP,
    NUKE_DETECTED_PLAYER,
    NUKE_NOT_DETECTED_PLAYER,
    NUKE_DEFUSED
}

public static class EVENT_BUS
{
    private static readonly Dictionary<EventType, Action> eventDictionary = new Dictionary<EventType, Action>();

    public static void Subscribe(EventType eventType, Action listener)
    {
        if (!eventDictionary.ContainsKey(eventType))
        {
            eventDictionary[eventType] = null;
        }

        eventDictionary[eventType] += listener;
    }

    public static void Unsubscribe(EventType eventType, Action listener)
    {
        if (eventDictionary.ContainsKey(eventType))
        {
            eventDictionary[eventType] -= listener;
        }
    }

    public static void Publish(EventType eventType)
    {
        if (eventDictionary.TryGetValue(eventType, out Action thisEvent))
        {
            thisEvent?.Invoke();
            //Debug.Log($"Event invoked: {eventType}");
        }
    }
}
