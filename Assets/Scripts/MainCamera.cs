using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Rigidbody2D pc;
    public int DistanceAway = 50;
    public float minimumY = 0;
    private float smoothTime = 0.075f;
    private Vector3 cameraVelocity = Vector3.zero;
    [SerializeField] private float offsetY = 0f;

    //public Camera camera;
    void FixedUpdate()
    {
        Vector3 PlayerPos = pc.transform.transform.position;
        GetComponent<Camera>().transform.position = Vector3.SmoothDamp(GetComponent<Camera>().transform.position,
                                                                       new Vector3(PlayerPos.x, Mathf.Clamp(PlayerPos.y, minimumY, float.MaxValue) + offsetY, PlayerPos.z - DistanceAway),
                                                                       ref cameraVelocity,
                                                                       smoothTime
                                                                      );
    }
}


