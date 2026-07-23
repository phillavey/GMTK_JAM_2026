using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    
    private Camera mainCamera;

    [SerializeField] private SpriteRenderer itemRenderer;
    [SerializeField] private Sprite plungerSprite;
    [SerializeField] private Sprite sageStickSprite;
    [SerializeField] private Animator playerAnimator;

    public bool swinging;

    public enum ItemType
    {
        NONE,
        PLUNGER,
        SAGE_STICK
    }

    public ItemType CurrentItem { get; private set; } = ItemType.NONE;

    void Start()
    {
        playerAnimator.enabled = false;

        mainCamera = Camera.main;

        EVENT_BUS.Subscribe(EventType.ATTACK, Attack);
    }

    void Update()
    {
        PointItemAtMouse();
    }

    public void PickUpItem(ItemType item)
    {
        CurrentItem = item;
        SetItemSprite(item);
        Debug.Log($"Picked up: {item}");
    }

    public void DropItem(ItemType item)
    {
        CurrentItem = ItemType.NONE;
        SetItemSprite(ItemType.NONE);
        Debug.Log($"Dropped: {item}");
    }

    public void SetItemSprite(ItemType item)
    {
        switch (item)
        {
            case ItemType.NONE:
                itemRenderer.sprite = null;
                break;
            case ItemType.PLUNGER:
                itemRenderer.sprite = plungerSprite;
                break;
            case ItemType.SAGE_STICK:
                itemRenderer.sprite = sageStickSprite;
                break;

        }        
    }

    private void PointItemAtMouse()
    {
        if (!swinging)
        {
            float angle = GetAngleFromPlayerToMouse();
            transform.localEulerAngles = new Vector3(0, 0, angle - 135);
        }
    }

    private float GetAngleFromPlayerToMouse()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        mousePos = mainCamera.ScreenToWorldPoint(mousePos);
        return Mathf.Atan2(mousePos.y - transform.parent.position.y, mousePos.x - transform.parent.position.x) * Mathf.Rad2Deg;
    }

    private void Attack()
    {
        switch (CurrentItem)
        {
            case ItemType.PLUNGER:
                //swinging = true;
                StartCoroutine(DoSwingAnimation());
                //swinging = false;
                break;
            case ItemType.SAGE_STICK:
                break;
            case ItemType.NONE:
                break;
        }
    }

    IEnumerator DoSwingAnimation()
    {
        Debug.Log("ATTACKED!!!");

        playerAnimator.enabled = true;
        playerAnimator.SetTrigger("player_swing");
        yield return new WaitForSeconds(0.2f);
        playerAnimator.enabled = false;
    }
}
