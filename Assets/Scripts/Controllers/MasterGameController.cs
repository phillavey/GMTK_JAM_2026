using System;
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
    [Tooltip("Ghost Prefab for spawning ghosts event.")]
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private GameObject bob; /* <- prefab */
    [SerializeField] private GameObject toiletPrefab;
    [SerializeField] private Light2D globalLight;

    private System.Random random;
    private Vector2 taskSpawnPoint;

    public enum GAME_TASK
    {
        GHOSTBUSTING,
        NUKE_DIFFUSING,
        TOILET_PLUNGING,
        DISPELLING_DARKNESS,
        FEEDING_BOB,
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
            SpawnTask(GAME_TASK.TOILET_PLUNGING, ROOM.BATHROOM);
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
        // EVENT_BUS.Publish(EventType.EVENT_STARTED, task, room);
        // ^ Would be soooo nice
        Debug.LogWarning($"SPAWNED TASK {task} IN ROOM {room}!");
        SetTaskSpawnPointForRoom(room);
        switch (task)
        {
            case GAME_TASK.GHOSTBUSTING:
                SpawnGhostbustingTask();
                break;
            case GAME_TASK.NUKE_DIFFUSING:
                SpawnGhostbustingTask();
                break;
            case GAME_TASK.TOILET_PLUNGING:
                SpawnToiletPlungingTask();
                break;
            case GAME_TASK.DISPELLING_DARKNESS:
                SpawnDispellingDarknessTask();
                break;
            case GAME_TASK.FEEDING_BOB:
                SpawnFeedBobTask();
                break;
        }
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
        // Amount of ghosts to spawn can be changed here
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
