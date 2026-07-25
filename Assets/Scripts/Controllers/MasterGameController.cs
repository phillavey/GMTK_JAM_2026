using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class MasterGameController : MonoBehaviour
{
    [Header("Locations")]
    [Tooltip("Where the game should spawn tasks in the living room.")]
    public Vector2 LivingRoomCenter;
    [Tooltip("Where the game should spawn tasks in the bed room.")]
    public Vector2 BedRoomCenter;
    [Tooltip("Where the game should spawn tasks in the kitchen.")]
    public Vector2 KitchenCenter;
    [Tooltip("Where the game should spawn tasks in the purple room.")]
    public Vector2 PurpleRoomCenter;
    [Tooltip("Where the game should spawn tasks in the bath room.")]
    public Vector2 BathRoomCenter;
    [Tooltip("Where the game should spawn tasks in the bath room.")]
    public Vector2 SeanceRoomCenter;

    [Header("Spawnables")]
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private GameObject bob; /* <- prefab */
    [SerializeField] private GameObject toiletPrefab;
    [SerializeField] private Light2D globalLight;

    private System.Random random;
    private Vector2 taskSpawnPoint;

    private static int TASK_ID = 0;
    // Track active spawned task enemies by key "TASK_ROOM" -> count
    private System.Collections.Generic.Dictionary<string, int> activeTaskCounts = new System.Collections.Generic.Dictionary<string, int>();

    public enum GAME_TASK
    {
        GHOSTBUSTING,
        NUKE_DIFFUSING,
        TOILET_PLUNGING,
        //DISPELLING_DARKNESS,
        FEEDING_BOB,
        BUBBLE_POPPING,
    }

    public enum ROOM
    {
        SEANCE_ROOM,
        LIVING_ROOM,
        KITCHEN,
        BATHROOM,
        BEDROOM,
        PURPLE_ROOM
    }

    void Awake()
    {
        taskSpawnPoint = Vector2.zero;
        random = new System.Random();
        EVENT_BUS.Subscribe(EventType.EVENT_STARTED, SpawnRandomTask);
        EVENT_BUS.Subscribe(EventType.ENEMY_KILLED, OnEnemyKilled);
        EVENT_BUS.Subscribe(EventType.TIMER_FINISHED, DoGameOver);
    }

    private string CountKey(GAME_TASK task, ROOM room) => $"{task}_{room}";

    private void IncrementSpawnedCount(GAME_TASK task, ROOM room, int amount)
    {
        string key = CountKey(task, room);
        if (activeTaskCounts.ContainsKey(key)) activeTaskCounts[key] += amount;
        else activeTaskCounts[key] = amount;
    }

    private void OnEnemyKilled(PublishEventArgs args)
    {
        if (args == null || args.Data == null) return;
        args.Data.TryGetValue("task_name", out object taskObj);
        args.Data.TryGetValue("task_room", out object roomObj);
        if (taskObj == null || roomObj == null) return;

        if (taskObj is GAME_TASK task && roomObj is ROOM room)
        {
            string key = CountKey(task, room);
            if (!activeTaskCounts.ContainsKey(key)) return;
            activeTaskCounts[key] -= 1;
            if (activeTaskCounts[key] <= 0)
            {
                activeTaskCounts.Remove(key);
                // Publish TASK_COMPLETED for this task/room
                var eventArgs = new System.Collections.Generic.Dictionary<string, object>()
                {
                    { "task_name", task },
                    { "task_room", room }
                };
                EVENT_BUS.Publish(EventType.TASK_COMPLETED, new PublishEventArgs(eventArgs));
            }
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
        {
            Debug.Log("SPAWNING TASK");
            SpawnRandomTask(null);
            //SpawnTask(GAME_TASK.NUKE_DIFFUSING, ROOM.LIVING_ROOM);
        }
    }

    void SpawnRandomTask(PublishEventArgs args)
    {
        Array tasks = Enum.GetValues(typeof(GAME_TASK));
        Array rooms = Enum.GetValues(typeof(ROOM));
        GAME_TASK randomTask = (GAME_TASK)tasks.GetValue(random.Next(tasks.Length));
        ROOM randomRoom = (ROOM)rooms.GetValue(random.Next(rooms.Length));
        SpawnTask(randomTask, randomRoom);
    }

    void SpawnTask(GAME_TASK task, ROOM room)
    {
        // For some tasks (like the nuke) we don't expose the room in the UI
        Dictionary<string, object> eventArgs;
        if (task == GAME_TASK.NUKE_DIFFUSING || task == GAME_TASK.TOILET_PLUNGING)
        {
            eventArgs = new Dictionary<string, object>()
            {
                { "task_name", task },
                { "task_id", TASK_ID++ }
            };
        }
        else
        {
            eventArgs = new Dictionary<string, object>()
            {
                { "task_name", task },
                { "task_id", TASK_ID++ },
                { "task_room", room }
            };
        }
        EVENT_BUS.Publish(EventType.NOTIFY_UI_EVENT_STARTED, new PublishEventArgs(eventArgs));

        SetTaskSpawnPointForRoom(room);
        switch (task)
        {
            case GAME_TASK.GHOSTBUSTING:
                SpawnGhostbustingTask(room);
                break;
            case GAME_TASK.NUKE_DIFFUSING:
                EVENT_BUS.Publish(EventType.NUKEING_IS_NOW_LEGAL, null);
                break;
            case GAME_TASK.TOILET_PLUNGING:
                SpawnToiletPlungingTask(room);
                break;
            //case GAME_TASK.DISPELLING_DARKNESS:
            //    SpawnDispellingDarknessTask();
            //    break;
            case GAME_TASK.FEEDING_BOB:
                SpawnFeedBobTask(room);
                break;
            case GAME_TASK.BUBBLE_POPPING:
                SpawnBubblePoppingTask(room);
                break;
        }
        Debug.LogWarning($"SPAWNED TASK {task} IN ROOM {room}!");
    }

    void SetTaskSpawnPointForRoom(ROOM room)
    {
        switch (room)
        {
            case ROOM.LIVING_ROOM:
                taskSpawnPoint = LivingRoomCenter;
                break;
            case ROOM.KITCHEN:
                taskSpawnPoint = KitchenCenter;
                break;
            case ROOM.BATHROOM:
                taskSpawnPoint = BathRoomCenter;
                break;
            case ROOM.BEDROOM:
                taskSpawnPoint = BedRoomCenter;
                break;
            case ROOM.PURPLE_ROOM:
                taskSpawnPoint = PurpleRoomCenter;
                break;
            case ROOM.SEANCE_ROOM:
                taskSpawnPoint = SeanceRoomCenter;
                break;
            default:
                taskSpawnPoint = LivingRoomCenter;
                break;
        }
    }

    void SpawnGhostbustingTask(ROOM room)
    {
        // Amount of bubbles to spawn can be changed here
        int numGhosts = 30;

        GameObject[] ghosts = new GameObject[numGhosts];
        float distanceAway;
        for (int i = 0; i < numGhosts; i++)
        {
            distanceAway = (float) random.NextDouble() * 10;
            GameObject ghost = Instantiate(ghostPrefab);
            GhostEnemy ge = ghost.GetComponent<GhostEnemy>();
            if (ge != null) ge.taskRoom = room;
            ghost.transform.position = new Vector3(
                ((float) (random.NextDouble() * 3 - 1.5f) * distanceAway) + taskSpawnPoint.x,
                ((float) (random.NextDouble() * 3 - 1.5f) * distanceAway) + taskSpawnPoint.y,
                0
            );
            ghosts[i] = ghost;
        }

        // Setup ^, then activate
        foreach (GameObject ghost in ghosts)
        {
            ghost.SetActive(true);
        }
        // Track how many ghosts we spawned for this task/room so we can notify UI when they're all dead
        IncrementSpawnedCount(GAME_TASK.GHOSTBUSTING, room, numGhosts);
    }

    void SpawnBubblePoppingTask(ROOM room)
    {
        int numBubbles = 1;

        GameObject[] bubbles = new GameObject[numBubbles];
        float distanceAway;
        for (int i = 0; i < numBubbles; i++)
        {
            distanceAway = (float)random.NextDouble() * 2;
            GameObject bubble = Instantiate(bubblePrefab);
            DirtyBubble db = bubble.GetComponent<DirtyBubble>();
            if (db != null) db.taskRoom = room;
            bubble.transform.position = new Vector3(
                ((float)(random.NextDouble() * 3 - 1.5f) * distanceAway) + taskSpawnPoint.x,
                ((float)(random.NextDouble() * 3 - 1.5f) * distanceAway) + taskSpawnPoint.y,
                0
            );
            bubbles[i] = bubble;
        }

        // Setup ^, then activate
        foreach (GameObject bubble in bubbles)
        {
            bubble.SetActive(true);
        }
        // Track bubble count for UI completion
        IncrementSpawnedCount(GAME_TASK.BUBBLE_POPPING, room, numBubbles);
    }

    void SpawnDispellingDarknessTask()
    {
        if (globalLight != null)
        {
            globalLight.intensity = 0.05f;
        }
    }

    void SpawnFeedBobTask(ROOM room)
    {
        GameObject bobington = Instantiate(bob);
        Bob bComp = bobington.GetComponent<Bob>();
        if (bComp != null) bComp.taskRoom = room;

        bobington.transform.position = new Vector3(taskSpawnPoint.x, taskSpawnPoint.y, 0);

        bobington.SetActive(true);
    }

    void SpawnToiletPlungingTask(ROOM room)
    {
        // Could technically spawn toilet in dup locations, but that's an ok bug I think
        float toiletSpawnHeight = 28.47f;
        float toiletSpawnWidthDiff = 4f;
        float firstToiletX = -101.48f;
        int numStalls = 24; // Stall 19 isn't valid

        int whichToilet = random.Next(1, numStalls + 1);
        whichToilet = whichToilet == 19 ? 1 : whichToilet; 
        float offsetX = firstToiletX + ((whichToilet - 1) * toiletSpawnWidthDiff);

        GameObject toilet = Instantiate(toiletPrefab);
        ToiletTarget tt = toilet.GetComponent<ToiletTarget>();
        if (tt != null) tt.taskRoom = room;

        toilet.transform.position = new Vector3(offsetX, toiletSpawnHeight, 0f);
        Debug.Log($"Spawning toilet in stall #{whichToilet} at pos {toilet.transform.position.x}");
        
        toilet.SetActive(true);
    }

    void DoGameOver(PublishEventArgs args)
    {
        Time.timeScale = 0f;
    }
}
