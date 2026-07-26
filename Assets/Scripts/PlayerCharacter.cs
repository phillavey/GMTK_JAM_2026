using Unity.Cinemachine;
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
    [SerializeField] private SpriteRenderer flameRenderer;
    [SerializeField] private GameObject playerInventory;
    [SerializeField] private CinemachineImpulseSource impulseSource;

    // Code-only stuff (NEVER accessed via the unity editor)
    private InputAction attack;
    private InputAction throw_;
    private InputAction move;
    private InputAction sprint;
    private float sprintCounter;
    private float boostSpeed;
    private Vector2 moveValue;
    private float horizontal = 0;
    private float vertical = 0;
    private Spellcasting spellScript;
    private PlayerInventory inventory;

    void Start()
    {
        this.attack = InputSystem.actions.FindAction("Attack");
        this.move = InputSystem.actions.FindAction("Move");
        this.throw_ = InputSystem.actions.FindAction("Throw");
        this.sprint = InputSystem.actions.FindAction("Sprint");
        spellScript = GetComponentInChildren<Spellcasting>();
        inventory = GetComponentInChildren<PlayerInventory>();

        MusicController.instance?.PlayMusic("main_theme");
    }

    void FixedUpdate()
    {
        if (boostSpeed > 1f)
        {
            rigidbody.AddRelativeForce(new Vector2(horizontal * horizontalMoveSpeed * sprintControlFactor, vertical * verticalMoveSpeed * sprintControlFactor));
            rigidbody.linearVelocity = Vector2.ClampMagnitude(rigidbody.linearVelocity, boostSpeed * 10);
        }
    }

    void Update()
    {
        // Stuff that doesn't need to be sync'd to the game clock
        DoAnimations();
        ReactToPlayerInput();
        if (sprintCounter > 0)
        {
            flameRenderer.enabled = true;
            sprintCounter = Mathf.Clamp(sprintCounter - Time.deltaTime, 0f, 1000f);
        }
        else
        {
            flameRenderer.enabled = false;
            boostSpeed = 1f;
        }
    }

    private void ReactToPlayerInput()
    {
        // Recording inputs should not sync with game clock
        moveValue = move.ReadValue<Vector2>();
        horizontal = moveValue.normalized.x;
        vertical = moveValue.normalized.y;

        sprite.flipX = horizontal < 0;

        if (attack.WasPressedThisFrame() && inventory.CurrentItem != PlayerInventory.ItemType.NONE) {
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

        if (boostSpeed <= 1f)
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

        SFX_Controller.instance.playSFX("zoom", transform, .3f);
        EVENT_BUS.Publish(EventType.SPRINTED, null);
        impulseSource.GenerateImpulse();
        sprintCounter = sprintDuration;
        boostSpeed = sprintSpeed;

        rigidbody.AddRelativeForce(new Vector2(h * boostSpeed * 10, v * boostSpeed * 10), ForceMode2D.Impulse);
    }
}
