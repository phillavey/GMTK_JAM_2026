using UnityEngine;

public class OnEnterSeanceRoom : MonoBehaviour
{
    [SerializeField] private GameObject camera;
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entered " + collision.gameObject.layer);
        // Player layer
        if (collision.gameObject.layer.Equals(12))
        {
            camera.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.layer);
        // Player layer
        if (collision.gameObject.layer.Equals(12))
        {
            camera.SetActive(false);
        }
    }
}
