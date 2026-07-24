using UnityEngine;
using UnityEngine.InputSystem;
using static MasterGameController;

public class PlayerCharacter : MonoBehaviour
{
    // Editable in the unity editor
    public float speedLimit = 20f;
    public float verticalMoveSpeed = 30f;
    public float horizontalMoveSpeed = 30f;

    // Serialized Fields (other objects attached via the editor)
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private GameObject playerInventory;

    // Code-only stuff (NEVER accessed via the unity editor)
    private InputAction attack;
    private InputAction throw_;
    private InputAction move;
    private Vector2 moveValue;

    void Start()
    {
        this.attack = InputSystem.actions.FindAction("Attack");
        this.move = InputSystem.actions.FindAction("Move");
        this.throw_ = InputSystem.actions.FindAction("Throw");

        MusicController.instance?.PlayMusic("main_theme");
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

        if (attack.WasPressedThisFrame()) {
            EVENT_BUS.Publish(EventType.ATTACK, null);
        }

        if (throw_.WasPressedThisFrame())
        {
            EVENT_BUS.Publish(EventType.THROW_ITEM, null);
        }
    }

    private void DoAnimations()
    {
        // Animation code...
    }
}
