using UnityEngine;

public class Bob : MonoBehaviour, IEnemy
{
    public void HitWithWeapon(PlayerInventory.ItemType weapon)
    {
        // One shot kill
        EVENT_BUS.Publish(EventType.BOB_FED, null);
        Destroy(this.gameObject);
    }
}
