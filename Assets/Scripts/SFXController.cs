using System;
using UnityEngine;

public class SFXController : MonoBehaviour
{
    void Start()
    {
        EVENT_BUS.AddEventlistener(HandleJumpSFX);
    }

    void HandleJumpSFX(object obj, EventArgs ea)
    {
        if (EVENT_TYPES.JUMP.Equals(obj))
        {
            Debug.Log("HandleJumpSFX Fired!!!");
        }
    }
}
