using UnityEngine;

public class GhostEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject playerCharacter;
    private SpriteRenderer spriteRenderer;

    public float moveSpeed = 0.2f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerCharacter.transform.position, moveSpeed);
        spriteRenderer.flipX = playerCharacter.transform.position.x < transform.position.x;

        Vector3 tempPos = transform.position;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * 5f) * 0.015f;
        GetComponent<SpriteRenderer>().transform.position = tempPos;
    }

    public void HitWithWeapon(PlayerInventory.ItemType weapon)
    {
        // One shot kill
        EVENT_BUS.Publish(EventType.ENEMY_KILLED, null);
        Destroy(this.gameObject);
    }
}
