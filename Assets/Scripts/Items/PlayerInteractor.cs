using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInventory))]
public class PlayerInteractor : MonoBehaviour
{
    private PlayerInventory inventory;
    private IInteractable currentInteractable;
    private InputAction throw_;

    private void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
        this.throw_ = InputSystem.actions.FindAction("Throw");
    }

    private void Update()
    {
        if (throw_.WasPressedThisFrame())
        {
            if (currentInteractable != null)
            {
                currentInteractable.Interact(inventory);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();
        if (interactable != null)
        {
            currentInteractable = interactable;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();
        if (interactable != null && currentInteractable == interactable)
        {
            currentInteractable = null;
        }
    }
}