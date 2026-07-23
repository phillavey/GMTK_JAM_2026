using System;
using UnityEngine;
using UnityEngine.UIElements; // Required for UI Toolkit

public class UIClock : MonoBehaviour
{
    private Label timerLabel;

    void OnEnable()
    {
        // 1. Get the UIDocument component attached to this GameObject
        var uiDocument = GetComponent<UIDocument>();

        // 2. Find the Label inside the UI document by its name in the UXML
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