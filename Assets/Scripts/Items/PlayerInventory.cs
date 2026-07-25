using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    
    private Camera mainCamera;

    [SerializeField] private SpriteRenderer itemRenderer;
    [Header("Held Item Sprites")]
    [SerializeField] private Sprite plungerSprite;
    [SerializeField] private Sprite sageStickSprite;
    [SerializeField] private Sprite foodSprite;
    [SerializeField] private Sprite lanternSprite;
    [SerializeField] private Sprite scissorsSprite;

    [Header("Prefabs to spawn when thrown")]
    [SerializeField] private GameObject sageStickPrefab;
    [SerializeField] private GameObject lanternPrefab;
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private GameObject plungerPrefab;
    [SerializeField] private GameObject scissorsPrefab;

    [SerializeField] private Collider2D spellCastingAreaCollider;

    private Spellcasting spellcasting;
    private BoxCollider2D inventoryCollider;
    private Vector3 swingScalar;

    public bool swinging;
    private float swingProgress = 0f;

    public enum ItemType
    {
        NONE,
        PLUNGER,
        SAGE_STICK,
        LANTERN,
        FOOD,
        SCISSORS,
    }

    public ItemType CurrentItem { get; private set; } = ItemType.NONE;

    void Start()
    {
        mainCamera = Camera.main;
        swingScalar = itemRenderer.transform.localScale * 1.4f;

        EVENT_BUS.Subscribe(EventType.ATTACK, Attack);
        EVENT_BUS.Subscribe(EventType.THROW_ITEM, DoThrowItem);

        spellcasting = GetComponent<Spellcasting>();
        inventoryCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if(swinging)
        {
            swingProgress += 7f * Time.deltaTime;
            if (swingProgress >= 1f)
            {
                swinging = false;
                swingProgress = 0f;
                itemRenderer.transform.localScale = Vector3.one;
                return;
            }

            float angle = GetAngleFromPlayerToMouse();
            Vector3 mouseMinus45 = new Vector3(0, 0, angle - 180);
            Vector3 mousePlus45 = new Vector3(0, 0, angle - 90);
            transform.localEulerAngles = Vector3.Lerp(mousePlus45, mouseMinus45, swingProgress); 
        }
        else
        { 
            PointItemAtMouse();
        }
    }

    public void PickUpItem(ItemType item)
    {
        CurrentItem = item;
        SetItemSprite(item);
    }

    public void DropItem(ItemType item)
    {
        CurrentItem = ItemType.NONE;
        SetItemSprite(ItemType.NONE);
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
            case ItemType.LANTERN:
                itemRenderer.sprite = lanternSprite;
                break;
            case ItemType.FOOD:
                itemRenderer.sprite = foodSprite;
                break;
            case ItemType.SCISSORS:
                itemRenderer.sprite = scissorsSprite;
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

    private void Attack(PublishEventArgs args)
    {
        if (CurrentItem != ItemType.NONE)
        {
            swinging = true;
            itemRenderer.transform.localScale = swingScalar;

            if (inventoryCollider.IsTouching(spellCastingAreaCollider))
            {
                spellcasting.ApplySeanceSwing();
            }
        }
    }

    void DoThrowItem(PublishEventArgs args)
    {
        itemRenderer.transform.localScale = Vector3.one;
        GameObject thrownItem = null;
        switch (CurrentItem)
        {
            case ItemType.SAGE_STICK:
                thrownItem = Instantiate(sageStickPrefab);
                StartCoroutine(throwItem(thrownItem));
                break;
            case ItemType.FOOD:
                thrownItem = Instantiate(foodPrefab);
                StartCoroutine(throwItem(thrownItem));
                break;
            case ItemType.LANTERN:
                thrownItem = Instantiate(lanternPrefab);
                EVENT_BUS.Publish(EventType.LANTERN_DROPPED, null);
                StartCoroutine(throwItem(thrownItem));
                break;
            case ItemType.PLUNGER:
                thrownItem = Instantiate(plungerPrefab);
                StartCoroutine(throwItem(thrownItem));
                break;
            case ItemType.SCISSORS:
                thrownItem = Instantiate(scissorsPrefab);
                StartCoroutine(throwItem(thrownItem));
                break;
            case ItemType.NONE:
                return;
        }
    }

    private IEnumerator throwItem(GameObject thrownItem)
    {
        thrownItem.SetActive(true);
        Rigidbody2D body = thrownItem.GetComponent<Rigidbody2D>();
        BoxCollider2D collider = thrownItem.GetComponent<BoxCollider2D>();

        float angle = GetAngleFromPlayerToMouse() * Mathf.Deg2Rad;
        float throwForce = 1100f;
        Vector2 spawnPoint = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        thrownItem.transform.position = transform.position + new Vector3(spawnPoint.x, spawnPoint.y, 0);
        body.bodyType = RigidbodyType2D.Dynamic;
        body.gravityScale = 0f;
        body.mass = 1f;
        collider.isTrigger = false;
        body.freezeRotation = false;
        body.angularVelocity = -800f;

        body.AddForce(spawnPoint * throwForce);
        DropItem(CurrentItem);

        yield return new WaitForSeconds(0.75f);
        body.linearVelocity = Vector2.zero;
        body.freezeRotation = true;
        body.rotation = 0f;

        body.bodyType = RigidbodyType2D.Kinematic;
        collider.isTrigger = true;
    }
}
