using System;
using UnityEngine;

public enum EVENT_TYPES
{
    JUMP,
    PLAYER_HURT,
}

public class EVENT_BUS : MonoBehaviour
{
    public static event EventHandler EventBus;

    public static void AddEventlistener(EventHandler eventListener)
    {
        EventBus += eventListener;
        Debug.Log("Added listener");
    }

    public static void RemoveEventlistener(EventHandler eventListener)
    {
        EventBus -= eventListener;
        Debug.Log("Removed listener");
    }

    public static void InvokeEvent(object eventType)
    {
        EventBus?.Invoke(eventType, EventArgs.Empty);
        Debug.Log("Event invoked: " + eventType);
    }
}
