using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIClock : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Image timerBar;

    private float maxTime = -1f;

    void Start()
    {
        if (GlobalTimer.Instance != null)
        {
            maxTime = GlobalTimer.Instance.TimeRemaining;
        }

        if (timerText != null)
        {
            timerText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (GlobalTimer.Instance == null)
        {
            return;
        }

        float timeRemaining = GlobalTimer.Instance.TimeRemaining;

        if (timerText != null && timerText.gameObject.activeSelf)
        {
            TimeSpan time = TimeSpan.FromSeconds(timeRemaining);
            timerText.text = time.ToString(@"mm\:ss");
        }

        if (timerBar != null)
        {
            if (maxTime <= 0f)
            {
                maxTime = GlobalTimer.Instance.TimeRemaining > 0f ? GlobalTimer.Instance.TimeRemaining : 1f;
            }

            float fill = maxTime > 0f ? Mathf.Clamp01(timeRemaining / maxTime) : 0f;
            timerBar.fillAmount = fill;
        }
    }
}
