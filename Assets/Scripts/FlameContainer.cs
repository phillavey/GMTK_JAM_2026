using UnityEngine;

public class FlameContainer : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;


    void Update()
    {
        Vector2 dir = rb.linearVelocity;
        transform.localEulerAngles = new Vector3(0, 0, (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) + 90);
    }
}
