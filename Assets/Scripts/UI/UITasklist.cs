using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static MasterGameController;

public class UITasklist : MonoBehaviour
{
    private VisualElement taskContainer;

    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        taskContainer = uiDocument.rootVisualElement.Q<VisualElement>("TaskListContainer");
        EVENT_BUS.Subscribe(EventType.NOTIFY_UI_EVENT_STARTED, AddTaskToList);
        EVENT_BUS.Subscribe(EventType.TASK_COMPLETED, RemoveTaskFromList);
    }

    void AddTaskToList(PublishEventArgs args)
    {
        args.Data.TryGetValue("task_name", out object taskName);
        args.Data.TryGetValue("task_id", out object taskId);

        if (taskName == null || taskId == null)
        {
            Debug.LogError($"ARGS NOT PASSED CORRECTLY: {taskName}, {taskId}");
        }
        
        Label taskLabel = new Label(taskName.ToString());
        taskLabel.name = $"{(int) taskId}";
        taskContainer.Add(taskLabel);
    }

    void RemoveTaskFromList(PublishEventArgs args)
    {
        args.Data.TryGetValue("task_name", out object taskName);
        List<Label> result = taskContainer.Query<Label>().Where(elem => elem.text == taskName.ToString()).ToList();

        taskContainer.Remove(result[0]);
    }
}
