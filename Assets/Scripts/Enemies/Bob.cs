using UnityEngine;

public class Bob : MonoBehaviour, IEnemy
{
    public void HitWithWeapon(PlayerInventory.ItemType weapon)
    {
        SFX_Controller.instance?.playSFX("whiff", transform, 1f);
        if (weapon == PlayerInventory.ItemType.FOOD)
        {
            // One shot kill
            EVENT_BUS.Publish(EventType.BOB_FED, null);
            Destroy(this.gameObject);
        } else
        {
            // Make bob mad??
        }
        
    }
}
