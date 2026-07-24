using UnityEngine;

public class PlungerPickup : MonoBehaviour, IInteractable
{
    public void Interact(PlayerInventory inventory)
    {
        if (inventory.CurrentItem != PlayerInventory.ItemType.NONE)
        {
            Debug.Log("Cannot pick up plunger, inventory is full.");
            return;
        }

        EVENT_BUS.Publish(EventType.ITEM_PICKUP, null);

        inventory.PickUpItem(PlayerInventory.ItemType.PLUNGER);

        Destroy(gameObject);
    }
}
