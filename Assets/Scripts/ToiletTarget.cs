using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using static MasterGameController;

public class ToiletTarget : MonoBehaviour, IEnemy
{
    // Room this spawned toilet belongs to (set by spawner)
    public ROOM taskRoom;
    private void Update()
    {
        float sizeScalar = Mathf.Sin(Time.fixedTime * Mathf.PI * 0.5f) * 0.00035f;
        Vector3 temp = new Vector3(
            Mathf.Clamp(gameObject.transform.localScale.x + sizeScalar, 1.1f, 2f)
            , Mathf.Clamp(gameObject.transform.localScale.y + sizeScalar, 1.1f, 2f)
            , Mathf.Clamp(gameObject.transform.localScale.z + sizeScalar, 1.1f, 2f)
            );
        gameObject.transform.localScale = temp;        
    }

    public void HitWithWeapon(PlayerInventory.ItemType weapon)
    {
        SFX_Controller.instance?.playSFX("whiff", transform, 1f);
        if (weapon == PlayerInventory.ItemType.PLUNGER)
        {
            // One shot kill
            EVENT_BUS.Publish(EventType.ENEMY_KILLED, null);
            Dictionary<string, object> eventArgs = new()
            {
                { "task_name", GAME_TASK.TOILET_PLUNGING }
            };
            EVENT_BUS.Publish(EventType.TASK_COMPLETED, new PublishEventArgs(eventArgs));
            Destroy(this.gameObject);
        }
        else
        {
            // Tell player they need a plunger?
        }
    }
}
