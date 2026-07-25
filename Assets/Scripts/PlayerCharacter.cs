using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{
    // Editable in the unity editor
    public float sprintSpeed = 20f;
    public float sprintDuration = 1.5f;
    public float sprintControlFactor;
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
    private InputAction sprint;
    private float sprintCounter;
    private float boostSpeed;
    private Vector2 moveValue;

    private Spellcasting spellScript;

    void Start()
    {
        this.attack = InputSystem.actions.FindAction("Attack");
        this.move = InputSystem.actions.FindAction("Move");
        this.throw_ = InputSystem.actions.FindAction("Throw");
        this.sprint = InputSystem.actions.FindAction("Sprint");
        spellScript = GetComponentInChildren<Spellcasting>();

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
        if (sprintCounter > 0)
            sprintCounter = Mathf.Clamp(sprintCounter - Time.deltaTime, 0f, 1000f);
        else
            boostSpeed = 1f;
    }

    private void ReactToPlayerInput()
    {
        // Recording inputs should not sync with game clock
        moveValue = move.ReadValue<Vector2>();
        float horizontal = moveValue.normalized.x;
        float vertical = moveValue.normalized.y;

        //rigidbody.linearVelocity = new Vector2(horizontal * horizontalMoveSpeed, vertical * verticalMoveSpeed);

        if (attack.WasPressedThisFrame()) {
            EVENT_BUS.Publish(EventType.ATTACK, null);
        }

        if (throw_.WasPressedThisFrame())
        {
            EVENT_BUS.Publish(EventType.THROW_ITEM, null);
        }

        if (sprint.WasPressedThisFrame())
        {
            SprintBoost(horizontal, vertical);
        }

        if (boostSpeed > 1f)
        {
            rigidbody.AddRelativeForce(new Vector2(horizontal * horizontalMoveSpeed * sprintControlFactor, vertical * verticalMoveSpeed * sprintControlFactor));
            rigidbody.linearVelocity = Vector2.ClampMagnitude(rigidbody.linearVelocity, boostSpeed * 10);
        }
        else
        {
            rigidbody.linearVelocity = new Vector2(horizontal * horizontalMoveSpeed, vertical * verticalMoveSpeed);

        }
    }

    private void DoAnimations()
    {
        // Animation code...
    }

    private void SprintBoost(float h, float v)
    {
        if (spellScript.focusMeter <= 0)
            return;

        EVENT_BUS.Publish(EventType.SPRINTED, null);
        Debug.Log("BOOSTING");
        sprintCounter += sprintDuration;
        boostSpeed = sprintSpeed;

        rigidbody.AddRelativeForce(new Vector2(h * boostSpeed * 10, v * boostSpeed * 10), ForceMode2D.Impulse);
    }
}
