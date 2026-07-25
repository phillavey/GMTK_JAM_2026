using UnityEngine;

public class PlungerPickup : MonoBehaviour, IInteractable
{
    public void Interact(PlayerInventory inventory)
    {
        inventory.PickUpItem(PlayerInventory.ItemType.PLUNGER);
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        Vector3 tempPos = transform.position;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * 2f) * 0.02f;
        GetComponent<SpriteRenderer>().transform.position = tempPos;
    }
}
