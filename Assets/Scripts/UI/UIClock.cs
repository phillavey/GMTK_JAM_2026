using System;
using UnityEngine;
using UnityEngine.UIElements; // Required for UI Toolkit

public class UIClock : MonoBehaviour
{
    private Label timerLabel;

    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        timerLabel = uiDocument.rootVisualElement.Q<Label>("TimerLabel");
    }

    void Update()
    {
        if (timerLabel == null)
        {
            return;
        }

        if (GlobalTimer.Instance == null)
        {
            return;
        }

        float timeRemaining = GlobalTimer.Instance.TimeRemaining;

        TimeSpan time = TimeSpan.FromSeconds(timeRemaining);

        timerLabel.text = time.ToString(@"mm\:ss");
    }
}