using UnityEngine;
using System.Collections.Generic;

public class GlobalTimer : MonoBehaviour
{
    public static GlobalTimer Instance { get; private set; }

    public float TimeRemaining { get; set; }
    public bool IsTimerRunning { get; private set; }
    public bool IsNukeTimerRunning { get; private set; }

    private float spellCastTimeIncreaseAmount = 30f;
    private float SpawnTimer = 3f;
    private float nukeTimer = 60f;
    private float lastPublishedNukeTime = -1f;

    void OnEnable()
    {
        EVENT_BUS.Subscribe(EventType.SPELL_CAST_SUCCESS, IncreaseTimeRemaining);
        EVENT_BUS.Subscribe(EventType.NUKEING_IS_NOW_LEGAL, StartNukeTimer);
        EVENT_BUS.Subscribe(EventType.NUKE_DEFUSED, StopNukeTimer);
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

    // Update is called once per frame
    void Update()
    {
        if (!IsTimerRunning)
        {
            return;
        }

        TimeRemaining -= Time.deltaTime;
        SpawnTimer -= Time.deltaTime;

        if (IsNukeTimerRunning)
        {
            nukeTimer -= Time.deltaTime;
            if (Mathf.Abs(nukeTimer - lastPublishedNukeTime) > 0.05f)
            {
                var data = new Dictionary<string, object> { { "time_remaining", nukeTimer } };
                EVENT_BUS.Publish(EventType.NUKE_TIMER_TICK, new PublishEventArgs(data));
                lastPublishedNukeTime = nukeTimer;
            }
        }

        if (SpawnTimer <= 0)
        {
            SpawnTimer = 10f;
            EVENT_BUS.Publish(EventType.EVENT_STARTED, null);
        }

        if (TimeRemaining <= 0)
        {
            // Game over
            TimeRemaining = 0;
            IsTimerRunning = false;
            EVENT_BUS.Publish(EventType.TIMER_FINISHED, null);
        }

        if (nukeTimer <= 0)
        {
            // Game over
            Debug.Log("NUKE SET OFF");
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

    void IncreaseTimeRemaining(PublishEventArgs args)
    {
        Debug.LogWarning("TIME INCREASED");
        TimeRemaining += spellCastTimeIncreaseAmount;
    }

    void StartNukeTimer(PublishEventArgs args)
    {
        if (!IsNukeTimerRunning)
        {
            nukeTimer = 60f;
            IsNukeTimerRunning = true;
        }
    }

    void StopNukeTimer(PublishEventArgs args)
    {
        IsNukeTimerRunning = false;
        nukeTimer = 60f;
    }
}
