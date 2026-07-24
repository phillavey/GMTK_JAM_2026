using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class ToiletTarget : MonoBehaviour, IInteractable
{
    private bool isClogged = true;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

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

        EVENT_BUS.Publish(EventType.TOILET_UNCLOGGED, null);

        Debug.Log("You unclogged the toilet!");

        inventory.DropItem(PlayerInventory.ItemType.PLUNGER);

        Debug.Log("The plunger broke!");
    }
}
