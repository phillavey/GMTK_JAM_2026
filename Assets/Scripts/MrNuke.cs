using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MrNuke : MonoBehaviour, IEnemy
{
    public bool isAngry;
    // Use this for initialization
    void Start()
    {
        isAngry = false;
        EVENT_BUS.Subscribe(EventType.NUKEING_IS_NOW_LEGAL, MakeAngry);
    }

    void MakeAngry(PublishEventArgs args)
    {
        // Play some SFX
        isAngry = true;
    }

    public void HitWithWeapon(PlayerInventory.ItemType weapon)
    {
        SFX_Controller.instance?.playSFX("whiff", transform, 1f);
        if (weapon == PlayerInventory.ItemType.SCISSORS && isAngry)
        {
            // One shot kill
            Debug.Log("NUKE DEFUSED");
            EVENT_BUS.Publish(EventType.NUKE_DEFUSED, null);
            EVENT_BUS.Publish(EventType.TASK_COMPLETED, null);
            isAngry = false;
        }
        else
        {
            // Increment an angriness meter?
        }
    }
}
