using UnityEngine;

public class SFXController : MonoBehaviour
{
    void OnEnable()
    {
        EVENT_BUS.Subscribe(EventType.ATTACK, HandleAttackSFX);
    }

    private void OnDisable()
    {
        EVENT_BUS.Unsubscribe(EventType.ATTACK, HandleAttackSFX);
    }

    void HandleAttackSFX()
    {
        Debug.Log("HandleAttackSFX Fired!!!");
    }
}
