using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ItemRenderer : MonoBehaviour
{
    private PlayerInventory playerInventory;

    private void Awake()
    {
        EVENT_BUS.Subscribe(EventType.LANTERN_PICKED_UP, LightLantern);
        EVENT_BUS.Subscribe(EventType.LANTERN_DROPPED, LightLantern);
    }

    private void Start()
    {
        if (playerInventory == null)
        {
            playerInventory = GetComponentInParent<PlayerInventory>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.LogError($"ENTERED COLLISION {collision}");
        IEnemy enemy = collision.GetComponent<IEnemy>();
        if (enemy != null && playerInventory.swinging)
        {
            enemy.HitWithWeapon(playerInventory.CurrentItem);
        }
    }

    private void LightLantern(PublishEventArgs args)
    {
        if (playerInventory.CurrentItem == PlayerInventory.ItemType.LANTERN)
        {
            Light2D lanternLight = GetComponentInChildren<Light2D>();
            if (lanternLight != null)
            {
                lanternLight.enabled = !lanternLight.enabled;
            }
        }
    }
}
