using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class PlayerCharacter : MonoBehaviour
{
    // Editable in the unity editor
    public float speedLimit = 20f;
    public float verticalMoveSpeed = 30f;
    public float horizontalMoveSpeed = 30f;

    // Serialized Fields (other objects attached via the editor)
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private SpriteRenderer sprite;

    // Code-only stuff (NEVER accessed via the unity editor)
    private InputAction jump;
    private InputAction move;
    private Camera mainCamera;
    private Vector2 moveValue;

    void Start()
    {
        this.jump = InputSystem.actions.FindAction("Jump");
        this.move = InputSystem.actions.FindAction("Move");
        this.mainCamera = Camera.main;
    }

    void FixedUpdate()
    {
        // Stuff that needs to be sync'd to the game clock
    }

    void Update()
    {
        // Stuff that doesn't need to be sync'd to the game clock
        DoAnimations();
        ReactToPlayerInput();
    }

    private void ReactToPlayerInput()
    {
        // Recording inputs should not sync with game clock
        moveValue = move.ReadValue<Vector2>();
        float horizontal = moveValue.normalized.x;
        float vertical = moveValue.normalized.y;

        rigidbody.linearVelocity = new Vector2(horizontal * horizontalMoveSpeed, vertical * verticalMoveSpeed);

        if (jump.WasPressedThisFrame()) {
            EVENT_BUS.Publish(EventType.JUMP);
            // Rest of jump code here
        }
    }

    private void DoAnimations()
    {
        // Animation code...
        LookAtMouse();
    }

    private void LookAtMouse()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        mousePos = mainCamera.ScreenToWorldPoint(mousePos);
        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
