using UnityEngine;
using TMPro;

// Simple scoring system
// - 1000 points for each spell cast (SPELL_CAST_SUCCESS)
// - 100 points for each completed task (TASK_COMPLETED)
// - 5 points every second while the game runs
public class UIScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private long score = 0;
    private float secondAccumulator = 0f;
    [SerializeField] private int pointsPerSecond = 5;

    void OnEnable()
    {
        EVENT_BUS.Subscribe(EventType.TASK_COMPLETED, OnTaskCompleted);
        EVENT_BUS.Subscribe(EventType.SPELL_CAST_SUCCESS, OnSpellCastSuccess);
    }

    void OnDisable()
    {
        EVENT_BUS.Unsubscribe(EventType.TASK_COMPLETED, OnTaskCompleted);
        EVENT_BUS.Unsubscribe(EventType.SPELL_CAST_SUCCESS, OnSpellCastSuccess);
    }

    void Start()
    {
        UpdateScoreText();
    }

    void Update()
    {
        secondAccumulator += Time.deltaTime;
        while (secondAccumulator >= 1f)
        {
            secondAccumulator -= 1f;
            AddScore(pointsPerSecond);
        }
    }

    private void OnTaskCompleted(PublishEventArgs args)
    {
        AddScore(100);
    }

    private void OnSpellCastSuccess(PublishEventArgs args)
    {
        AddScore(1000);
    }

    public void AddScore(long amount)
    {
        score += amount;
        UpdateScoreText();
    }

    public long GetScore() => score;

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"{score}";
        }
    }
}
