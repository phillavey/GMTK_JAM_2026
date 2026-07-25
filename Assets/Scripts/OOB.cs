using UnityEngine;

public class OOB : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject);
        GameObject obj = collision.gameObject;
        //TryGetComponent<Rigidbody2D>(out Rigidbody2D rb);
        //if (rb != null)
        //{
        //    rb.linearVelocity = Vector3.zero;
        //    rb.
        //}
        obj.SetActive(false);
        obj.transform.position = new Vector3(-50, -8, 0);
        obj.SetActive(true);
    }
}
