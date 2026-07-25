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
    public Vector2 BedRoomCenter = new Vector2(-69, -23);
    [Tooltip("Where the game should spawn tasks in the kitchen.")]
    public Vector2 KitchenCenter = new Vector2(-88, 7);
    [Tooltip("Where the game should spawn tasks in the purple room.")]
    public Vector2 PurpleRoomCenter = new Vector2(-23, -33);
    [Tooltip("Where the game should spawn tasks in the bath room.")]
    public Vector2 BathRoomCenter = new Vector2(-57, 27);

    [Header("Spawnables")]
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private GameObject bob; /* <- prefab */
    [SerializeField] private GameObject toiletPrefab;
    [SerializeField] private Light2D globalLight;

    private System.Random random;
    private Vector2 taskSpawnPoint;

    private static int TASK_ID = 0;

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
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
        {
            Debug.Log("SPAWNING TASK");
            //SpawnRandomTask(null);
            SpawnTask(GAME_TASK.FEEDING_BOB, ROOM.BATHROOM);
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
        Dictionary<string, object> eventArgs = new Dictionary<string, object>()
        {
            { "task_name", task },
            { "task_id", TASK_ID++ }
        };
        EVENT_BUS.Publish(EventType.NOTIFY_UI_EVENT_STARTED, new PublishEventArgs(eventArgs));
        
        SetTaskSpawnPointForRoom(room);
        switch (task)
        {
            case GAME_TASK.GHOSTBUSTING:
                SpawnGhostbustingTask();
                break;
            case GAME_TASK.NUKE_DIFFUSING:
                EVENT_BUS.Publish(EventType.NUKEING_IS_NOW_LEGAL, null);
                break;
            case GAME_TASK.TOILET_PLUNGING:
                SpawnToiletPlungingTask();
                break;
            //case GAME_TASK.DISPELLING_DARKNESS:
            //    SpawnDispellingDarknessTask();
            //    break;
            case GAME_TASK.FEEDING_BOB:
                SpawnFeedBobTask();
                break;
            case GAME_TASK.BUBBLE_POPPING:
                SpawnBubblePoppingTask();
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
            default:
                taskSpawnPoint = LivingRoomCenter;
                break;
        }
    }

    void SpawnGhostbustingTask()
    {
        // Amount of bubbles to spawn can be changed here
        int numGhosts = 30;

        GameObject[] ghosts = new GameObject[numGhosts];
        float distanceAway;
        for (int i = 0; i < numGhosts; i++)
        {
            distanceAway = (float) random.NextDouble() * 10;
            GameObject ghost = Instantiate(ghostPrefab);
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
    }

    void SpawnBubblePoppingTask()
    {
        int numBubbles = 5;

        GameObject[] bubbles = new GameObject[numBubbles];
        float distanceAway;
        for (int i = 0; i < numBubbles; i++)
        {
            distanceAway = (float)random.NextDouble() * 2;
            GameObject bubble = Instantiate(bubblePrefab);
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
    }

    void SpawnDispellingDarknessTask()
    {
        if (globalLight != null)
        {
            globalLight.intensity = 0.05f;
        }
    }

    void SpawnFeedBobTask()
    {
        GameObject bobington = Instantiate(bob);

        bobington.transform.position = new Vector3(taskSpawnPoint.x, taskSpawnPoint.y, 0);

        bobington.SetActive(true);
    }

    void SpawnToiletPlungingTask()
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

        toilet.transform.position = new Vector3(offsetX, toiletSpawnHeight, 0f);
        Debug.Log($"Spawning toilet in stall #{whichToilet} at pos {toilet.transform.position.x}");
        
        toilet.SetActive(true);
    }
}
