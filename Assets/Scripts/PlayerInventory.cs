using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;

    [SerializeField] private Sprite plungerSprite;
    [SerializeField] private Sprite sageStickSprite;
    public enum ItemType
    {
        NONE,
        PLUNGER,
        SAGE_STICK
    }

    public ItemType CurrentItem { get; private set; } = ItemType.NONE;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
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
                spriteRenderer.sprite = null;
                break;
            case ItemType.PLUNGER:
                //spriteRenderer.enabled = true;
                spriteRenderer.sprite = plungerSprite;
                break;
            case ItemType.SAGE_STICK:
                //spriteRenderer.enabled = true;
                spriteRenderer.sprite = sageStickSprite;
                break;

        }        
    }

    private void PointItemAtMouse()
    {
        float radius = .3f;
        Vector2 mousePos = Mouse.current.position.ReadValue();
        mousePos = mainCamera.ScreenToWorldPoint(mousePos);
        float angle = Mathf.Atan2(mousePos.y - transform.parent.position.y, mousePos.x - transform.parent.position.x) * Mathf.Rad2Deg;
        transform.localEulerAngles = new Vector3(0, 0, angle);
        float xpos = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float ypos = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

        transform.position = new Vector3(transform.parent.position.x + xpos, transform.parent.position.y + ypos, 0);
    }
}
