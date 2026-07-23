using UnityEngine;

public class GhostEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject playerCharacter;

    public float moveSpeed = 0.2f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerCharacter.transform.position, moveSpeed);
    }

    public void HitWithWeapon(PlayerInventory.ItemType weapon)
    {
        // One shot kill
        EVENT_BUS.Publish(EventType.ENEMY_KILLED);
        Destroy(this);
    }
}
