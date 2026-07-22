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
    private InputAction reset;
    private InputAction sprint;
    private Vector2 moveValue;

    void Start()
    {
        this.jump = InputSystem.actions.FindAction("Jump");
        this.move = InputSystem.actions.FindAction("Move");
        this.reset = InputSystem.actions.FindAction("Reset");
        this.sprint = InputSystem.actions.FindAction("Sprint");
    }

    void FixedUpdate()
    {
        // Stuff that needs to be sync'd to the game clock
        ReactToPlayerInput();
    }

    void Update()
    {
        // Stuff that doesn't need to be sync'd to the game clock
        DoAnimations();
    }

    private void ReactToPlayerInput()
    {
        // Recording inputs should sync with game clock so movement is consistent even with variable framerate
        moveValue = move.ReadValue<Vector2>();
        float horizontal = moveValue.normalized.x;
        float vertical = moveValue.normalized.y;

        rigidbody.linearVelocity = new Vector2(horizontal * horizontalMoveSpeed, vertical * verticalMoveSpeed);
    }

    private void DoAnimations()
    {
        // Animations can happen at variable framerate, so should not be sync'd to game clock

        // Animation code...
    }
}
