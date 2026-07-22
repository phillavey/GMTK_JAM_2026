using UnityEngine;

public class ToiletTarget : MonoBehaviour, IInteractable
{
    private bool isClogged = true;

    public void Interact(PlayerInventory inventory)
    {
        if (!isClogged)
        {
            Debug.Log("The toilet isn't clogged!");
            return;
        }

        if (inventory.CurrentItem != PlayerInventory.ItemType.PLUNGER)
        {
            Debug.Log("You need a plunger to unclog the toilet!");
            return;
        }

        isClogged = false;

        EVENT_BUS.Publish(EventType.TOILET_UNCLOGGED);

        Debug.Log("You unclogged the toilet!");

        inventory.DropItem(PlayerInventory.ItemType.PLUNGER);

        Debug.Log("The plunger broke!");
    }
}
