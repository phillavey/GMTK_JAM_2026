using UnityEngine;

public interface IEnemy
{
    void HitWithWeapon(PlayerInventory.ItemType weapon);

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("COLLIDED");
        EVENT_BUS.Publish(EventType.PLAYER_HURT);
    }
}
