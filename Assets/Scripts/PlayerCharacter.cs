using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private Vector2 moveValue;

    void Start()
    {
        this.jump = InputSystem.actions.FindAction("Jump");
        this.move = InputSystem.actions.FindAction("Move");
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
            EVENT_BUS.InvokeEvent(EVENT_TYPES.JUMP);
            // Rest of jump code here
        }
    }

    private void DoAnimations()
    {
        // Animation code...
    }
}
