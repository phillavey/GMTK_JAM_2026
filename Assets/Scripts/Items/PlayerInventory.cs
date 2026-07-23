using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    
    private Camera mainCamera;

    [SerializeField] private SpriteRenderer itemRenderer;
    [SerializeField] private Sprite plungerSprite;
    [SerializeField] private Sprite sageStickSprite;
    [SerializeField] private Animator playerAnimator;

    [SerializeField] private GameObject sageStickPrefab;

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
        EVENT_BUS.Subscribe(EventType.THOW_ITEM, DoThrowItem);
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
                swinging = true;
                StartCoroutine(DoSwingAnimation());
                swinging = false;
                break;
            case ItemType.SAGE_STICK:
                swinging = true;
                StartCoroutine(DoSwingAnimation());
                swinging = false;
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

    void DoThrowItem()
    {
        GameObject thrownItem = null;
        switch (CurrentItem)
        {
            case ItemType.PLUNGER:
                break;
            case ItemType.SAGE_STICK:
                //Debug.Log(GetAngleFromPlayerToMouse());
                thrownItem = Instantiate(sageStickPrefab);
                break;
            case ItemType.NONE:
                return;
        }

        

        thrownItem.SetActive(true);
        Rigidbody2D body = thrownItem.GetComponent<Rigidbody2D>();
        BoxCollider2D collider = thrownItem.GetComponent<BoxCollider2D>();

        float angle = GetAngleFromPlayerToMouse() * Mathf.Deg2Rad;
        float throwForce = 800f;
        Vector2 spawnPoint = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        //Debug.Log(spawnPoint);

        thrownItem.transform.position = transform.position + new Vector3(spawnPoint.x, spawnPoint.y, 0);
        body.bodyType = RigidbodyType2D.Dynamic;
        body.gravityScale = 0f;
        body.mass = 1f;
        collider.isTrigger = false;


        body.AddForce(spawnPoint * throwForce);
        DropItem(CurrentItem);

        StartCoroutine(wait(2));

        //body.bodyType = RigidbodyType2D.Kinematic;
        //collider.isTrigger = true;
    }

    private IEnumerator wait(int secs)
    {
        yield return new WaitForSeconds(secs);
    }
}
