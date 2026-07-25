using System.Collections.Generic;
using UnityEngine;
using static MasterGameController;

public class Bob : MonoBehaviour, IEnemy
{
    public ROOM taskRoom;
    public void HitWithWeapon(PlayerInventory.ItemType weapon)
    {
        SFX_Controller.instance?.playSFX("whiff", transform, 1f);
        if (weapon == PlayerInventory.ItemType.FOOD)
        {
            // One shot kill
            EVENT_BUS.Publish(EventType.BOB_FED, null);
            Dictionary<string, object> eventArgs = new()
            {
                { "task_name", GAME_TASK.FEEDING_BOB },
                { "task_room", taskRoom }
            };
            EVENT_BUS.Publish(EventType.TASK_COMPLETED, new PublishEventArgs(eventArgs));
            Destroy(this.gameObject);
        } else
        {
            // Make bob mad??
        }
        
    }
}
