using UnityEngine;

public class LanternPickup : MonoBehaviour, IInteractable
{
    public void Interact(PlayerInventory inventory)
    {
        if (inventory.CurrentItem != PlayerInventory.ItemType.NONE)
        {
            Debug.Log("Cannot pick up Lantern, inventory is full.");
            return;
        }

        inventory.PickUpItem(PlayerInventory.ItemType.LANTERN);
        EVENT_BUS.Publish(EventType.LANTERN_PICKED_UP, null);

        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        Vector3 tempPos = transform.position;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * 2f) * 0.02f;
        GetComponent<SpriteRenderer>().transform.position = tempPos;
    }

    public void Throw()
    {

    }
}
