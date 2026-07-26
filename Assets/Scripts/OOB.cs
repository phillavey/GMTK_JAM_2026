using UnityEngine;

public class OOB : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject);
        GameObject obj = collision.gameObject;
        //obj.SetActive(false);
        obj.transform.position = new Vector3(-50, -8, 0);
        //obj.SetActive(true);
    }
}
