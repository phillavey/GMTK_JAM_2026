using UnityEngine;
using TMPro;

public class NukeTimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    void Awake()
    {
        if (label != null) label.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        EVENT_BUS.Subscribe(EventType.NUKE_TIMER_TICK, OnNukeTimerTick);
        EVENT_BUS.Subscribe(EventType.NUKEING_IS_NOW_LEGAL, OnNukeStarted);
        EVENT_BUS.Subscribe(EventType.NUKE_DEFUSED, OnNukeStopped);
    }

    void OnDisable()
    {
        EVENT_BUS.Unsubscribe(EventType.NUKE_TIMER_TICK, OnNukeTimerTick);
        EVENT_BUS.Unsubscribe(EventType.NUKEING_IS_NOW_LEGAL, OnNukeStarted);
        EVENT_BUS.Unsubscribe(EventType.NUKE_DEFUSED, OnNukeStopped);
    }

    void OnNukeTimerTick(PublishEventArgs args)
    {
        if (label == null) return;
        if (!args.Data.TryGetValue("time_remaining", out object v)) return;
        float seconds = System.Convert.ToSingle(v);
        label.text = $"{seconds:0.0}s";
    }

    void OnNukeStarted(PublishEventArgs args)
    {
        if (label != null) label.gameObject.SetActive(true);
    }

    void OnNukeStopped(PublishEventArgs args)
    {
        if (label != null) label.gameObject.SetActive(false);
    }
}
