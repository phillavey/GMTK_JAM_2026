using UnityEngine;

public class ItemRenderer : MonoBehaviour
{
    private PlayerInventory playerInventory;

    private void Start()
    {
        if (playerInventory == null)
        {
            playerInventory = GetComponentInParent<PlayerInventory>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IEnemy enemy = collision.GetComponent<IEnemy>();
        if (enemy != null && playerInventory.swinging)
        {
            enemy.HitWithWeapon(playerInventory.CurrentItem);
        }
    }
}
