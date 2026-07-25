using UnityEngine;

public class DirtyBubble : MonoBehaviour, IEnemy
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    public float moveSpeed = 0.5f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //InvokeRepeating("fixVelocity", 5f, 5f);
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector3(10f * moveSpeed, 10f * moveSpeed, 10f * moveSpeed));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //fixVelocity();

        Vector3 tempPos = transform.position;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * 5f) * 0.015f;
        spriteRenderer.transform.position = tempPos;
    }

    public void HitWithWeapon(PlayerInventory.ItemType weapon)
    {
        SFX_Controller.instance?.playSFX("whiff", transform, 1f);
        if (weapon == PlayerInventory.ItemType.SCISSORS)
        {
            // One shot kill
            EVENT_BUS.Publish(EventType.ENEMY_KILLED, null);
            Destroy(this.gameObject);
        }
        else
        {
            // Maybe push them back
        }

    }

    void fixVelocity()
    {
        //Mathf.Clamp(rb.vel); // Maybe do this?
    }
}
