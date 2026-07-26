using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static MasterGameController;

public class UITasklist : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform taskContainer;
    [SerializeField] private GameObject taskLabelPrefab;
    [Header("Item icons (optional)")]
    [SerializeField] private Sprite plungerSprite;
    [SerializeField] private Sprite sageStickSprite;
    [SerializeField] private Sprite lanternSprite;
    [SerializeField] private Sprite scissorsSprite;
    [SerializeField] private Sprite foodSprite;

    private Dictionary<object, string> gameTaskToUIFriendlyName = new Dictionary<object, string>
    {
        { GAME_TASK.GHOSTBUSTING, "Ghosts appeared in the" },
        { GAME_TASK.TOILET_PLUNGING, "The toilet's clogged!" },
        { GAME_TASK.FEEDING_BOB, "Bob is hungry in the" },
        { GAME_TASK.BUBBLE_POPPING, "Dirty bubbles in the" },
        { GAME_TASK.NUKE_DIFFUSING, "Defuse Nuke Guy before he explodes!!!" }
    };

    private Dictionary<object, string> roomToUIFriendlyName = new Dictionary<object, string>
    {
        { ROOM.SEANCE_ROOM, "Seance Room" },
        { ROOM.LIVING_ROOM, "Living Room" },
        { ROOM.KITCHEN, "Kitchen" },
        { ROOM.BATHROOM, "Bathroom" },
        { ROOM.BEDROOM, "Bedroom" },
        { ROOM.PURPLE_ROOM, "Purple Room" }
    };

    void OnEnable()
    {
        EVENT_BUS.Subscribe(EventType.NOTIFY_UI_EVENT_STARTED, AddTaskToList);
        EVENT_BUS.Subscribe(EventType.TASK_COMPLETED, RemoveTaskFromList);
    }

    void OnDisable()
    {
        EVENT_BUS.Unsubscribe(EventType.NOTIFY_UI_EVENT_STARTED, AddTaskToList);
        EVENT_BUS.Unsubscribe(EventType.TASK_COMPLETED, RemoveTaskFromList);
    }

    void AddTaskToList(PublishEventArgs args)
    {
        args.Data.TryGetValue("task_name", out object taskName);
        args.Data.TryGetValue("task_id", out object taskId);
        args.Data.TryGetValue("task_room", out object taskRoom);

        if (taskName == null || taskId == null)
        {
            Debug.LogError($"ARGS NOT PASSED CORRECTLY: {taskName}, {taskId}, {taskRoom}");
            return;
        }

        if (gameTaskToUIFriendlyName.TryGetValue(taskName, out string friendlyDescription))
        {
            // Build label; include room only if provided
            string label = friendlyDescription;
            if (taskRoom != null && roomToUIFriendlyName.TryGetValue(taskRoom, out string friendlyRoomName))
            {
                label = friendlyDescription + " " + friendlyRoomName;
            }

            GameObject newTask = Instantiate(taskLabelPrefab, taskContainer);
            newTask.name = $"{(int)taskId}";

            TextMeshProUGUI textComp = newTask.GetComponentInChildren<TextMeshProUGUI>();
            if (textComp != null)
            {
                textComp.text = "• " + label;
                textComp.fontSize = 26;
            }

            var iconImage = newTask.GetComponentInChildren<UnityEngine.UI.Image>();
            if (iconImage != null)
            {
                Sprite icon = GetIconForTask((GAME_TASK)taskName);
                if (icon != null)
                {
                    iconImage.sprite = icon;
                    iconImage.enabled = true;
                }
                else
                {
                    // no icon for this task
                    iconImage.enabled = false;
                }
            }
        }
    }

    private Sprite GetIconForTask(GAME_TASK task)
    {
        switch (task)
        {
            case GAME_TASK.TOILET_PLUNGING:
                return plungerSprite;
            case GAME_TASK.GHOSTBUSTING:
                return sageStickSprite;
            case GAME_TASK.FEEDING_BOB:
                return foodSprite;
            case GAME_TASK.BUBBLE_POPPING:
                return scissorsSprite;
            case GAME_TASK.NUKE_DIFFUSING:
                return scissorsSprite;
            default:
                return null;
        }
    }

    void RemoveTaskFromList(PublishEventArgs args)
    {
        args.Data.TryGetValue("task_name", out object taskName);
        args.Data.TryGetValue("task_room", out object roomName);
        args.Data.TryGetValue("task_id", out object taskId);

        Debug.Log($"Removing task: {taskName} in {roomName} id={taskId}");

        // If a task_id was provided, prefer removing by instantiated GameObject name (set to taskId when created)
        if (taskId != null)
        {
            string targetName = $"{(int)taskId}";
            foreach (Transform child in taskContainer)
            {
                if (child.name == targetName)
                {
                    Destroy(child.gameObject);
                    return;
                }
            }
        }

        gameTaskToUIFriendlyName.TryGetValue(taskName, out string friendlyDescription);

        string friendlyRoomName = null;
        if (roomName != null)
        {
            roomToUIFriendlyName.TryGetValue(roomName, out friendlyRoomName);
        }

        if (!string.IsNullOrEmpty(friendlyRoomName))
        {
            // Text entries are prefixed with a bullet and a space
            string searchString = "\u2022 " + friendlyDescription + " " + friendlyRoomName;
            Debug.Log("Search String: " + searchString);
            foreach (Transform child in taskContainer)
            {
                TextMeshProUGUI textComp = child.GetComponentInChildren<TextMeshProUGUI>();
                if (textComp != null && textComp.text == searchString)
                {
                    Destroy(child.gameObject);
                }
            }
        }
        else if (!string.IsNullOrEmpty(friendlyDescription))
        {
            foreach (Transform child in taskContainer)
            {
                TextMeshProUGUI textComp = child.GetComponentInChildren<TextMeshProUGUI>();
                if (textComp != null && textComp.text != null && textComp.text.Contains(friendlyDescription))
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }
}