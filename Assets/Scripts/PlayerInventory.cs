using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public enum ItemType
    {
        NONE,
        PLUNGER,
        SAGE_STICK
    }

    public ItemType CurrentItem { get; private set; } = ItemType.NONE;

    public void PickUpItem(ItemType item)
    {
        CurrentItem = item;
        Debug.Log($"Picked up: {item}");
    }

    public void DropItem(ItemType item)
    {
        CurrentItem = ItemType.NONE;
        Debug.Log($"Dropped: {item}");
    }
}
