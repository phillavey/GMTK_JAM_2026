using UnityEngine;

public class GhostEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject playerCharacter;
    private SpriteRenderer spriteRenderer;

    public float moveSpeed = 0.2f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        InvokeRepeating("fixVelocity", 5f, 5f);
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
        SFX_Controller.instance?.playSFX("whiff", transform, 1f);
        if (weapon == PlayerInventory.ItemType.SAGE_STICK)
        {
            // One shot kill
            EVENT_BUS.Publish(EventType.ENEMY_KILLED, null);
            Destroy(this.gameObject);
        } else
        {
            // Maybe push them back
        }
        
    }

    void fixVelocity()
    {
        // Hacky? YES. It's a game jam
        //Debug.Log("FIXING GHOST VELOCITY");
        Rigidbody2D body = gameObject.GetComponent<Rigidbody2D>();
        body.linearVelocity = body.linearVelocity * 0.6f;
    }
}
