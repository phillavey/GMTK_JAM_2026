using UnityEngine;

public class TaskCompletionMock : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EVENT_BUS.Publish(EventType.TASK_COMPLETED, null);
            Debug.Log("Task Completed Event Published!");
        }
    }
}
