using System;
using Unity.VisualScripting;
using UnityEngine;

public class SFXController : MonoBehaviour
{
    void OnEnable()
    {
        EVENT_BUS.Subscribe(EventType.JUMP, HandleJumpSFX);
    }

    private void OnDisable()
    {
        EVENT_BUS.Unsubscribe(EventType.JUMP, HandleJumpSFX);
    }

    void HandleJumpSFX()
    {
        Debug.Log("HandleJumpSFX Fired!!!");
    }
}
