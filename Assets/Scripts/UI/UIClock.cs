using System;
using UnityEngine;
using TMPro; // Required for Canvas TextMeshPro elements

public class UIClock : MonoBehaviour
{
    // Expose the text component to the Unity Inspector
    [SerializeField] private TextMeshProUGUI timerText;

    void Update()
    {
        // Safety checks
        if (timerText == null || GlobalTimer.Instance == null)
        {
            return;
        }

        float timeRemaining = GlobalTimer.Instance.TimeRemaining;
        TimeSpan time = TimeSpan.FromSeconds(timeRemaining);

        // Update the Canvas text
        timerText.text = time.ToString(@"mm\:ss");
    }
}