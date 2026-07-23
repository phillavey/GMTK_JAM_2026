using UnityEngine;

public class SageStickPickup : MonoBehaviour, IInteractable
{
    public void Interact(PlayerInventory inventory)
    {
        if (inventory.CurrentItem != PlayerInventory.ItemType.NONE)
        {
            Debug.Log("Cannot pick up Sage Stick, inventory is full.");
            return;
        }

        inventory.PickUpItem(PlayerInventory.ItemType.SAGE_STICK);

        Destroy(gameObject);
    }
}
