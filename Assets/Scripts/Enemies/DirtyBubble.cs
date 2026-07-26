using System.Collections.Generic;
using UnityEngine;
using static MasterGameController;

public class DirtyBubble : MonoBehaviour, IEnemy
{
    public ROOM taskRoom;
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
            Dictionary<string, object> eventArgs = new()
            {
                { "task_name", GAME_TASK.BUBBLE_POPPING },
                { "task_room", taskRoom }
            };
            EVENT_BUS.Publish(EventType.ENEMY_KILLED, new PublishEventArgs(eventArgs));
            Destroy(this.gameObject);
        }
        else
        {
            // Maybe push them back
        }

    }
}
