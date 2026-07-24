using UnityEngine;

public class GlobalTimer : MonoBehaviour
{
    public static GlobalTimer Instance { get; private set; }

    public float TimeRemaining { get; set; }
    public bool IsTimerRunning { get; private set; }

    private float TaskCompletionIncreaseAmount = 15f;

    void OnEnable()
    {
        EVENT_BUS.Subscribe(EventType.TASK_COMPLETED, IncreaseTime);
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        TimeRemaining = 300f; // Set to 5 minutes (300 seconds)
        StartTimer();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsTimerRunning)
        {
            return;
        }

        TimeRemaining -= Time.deltaTime;

        if (TimeRemaining <= 0)
        {
            TimeRemaining = 0;
            IsTimerRunning = false;
            EVENT_BUS.Publish(EventType.TIMER_FINISHED, null);
        }
    }

    public void StartTimer()
    {
        EVENT_BUS.Publish(EventType.TIMER_STARTED, null);
        IsTimerRunning = true;
    }

    public void StopTimer()
    {
        EVENT_BUS.Publish(EventType.TIMER_STOPPED, null);
        IsTimerRunning = false;
    }

    void IncreaseTime(PublishEventArgs args)
    {
        TimeRemaining += TaskCompletionIncreaseAmount;
    }
}
