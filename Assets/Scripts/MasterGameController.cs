using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MasterGameController : MonoBehaviour
{
    [Header("Locations")]
    [Tooltip("Where the game should spawn tasks in the living room.")]
    public Vector2 LivingRoomCenter;
    //[Header("Locations")]
    //[Tooltip("Where the game should spawn tasks in the living room.")]
    //public Vector2 BedRoomCenter;
    //[Header("Locations")]
    //[Tooltip("Where the game should spawn tasks in the living room.")]
    //public Vector2 KitchenCenter;
    //[Header("Locations")]
    //[Tooltip("Where the game should spawn tasks in the living room.")]
    //public Vector2 LivingRoomCenter;
    //[Header("Locations")]
    //[Tooltip("Where the game should spawn tasks in the living room.")]
    //public Vector2 LivingRoomCenter;

    [Header("Spawnables")]
    [Tooltip("Ghost Prefab for spawning ghosts event.")]
    [SerializeField] private GameObject ghostPrefab;

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
    }

    void Awake()
    {
        taskSpawnPoint = Vector2.zero;
        random = new System.Random();
        EVENT_BUS.Subscribe(EventType.SPAWN_EVENT, SpawnRandomTask);
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
        {
            Debug.Log("SPAWNING TASK");
            SpawnTask(GAME_TASK.GHOSTBUSTING, ROOM.LIVING_ROOM);
        }
    }

    void SpawnRandomTask(PublishEventArgs args)
    {
        //Array tasks = Enum.GetValues(typeof(GAME_TASK));
        //Array rooms = Enum.GetValues(typeof(ROOM));
        //GAME_TASK randomTask = (GAME_TASK)tasks.GetValue(random.Next(tasks.Length));
        //ROOM randomRoom = (ROOM)rooms.GetValue(random.Next(rooms.Length));
        //SpawnTask(randomTask, randomRoom);
        Debug.Log("SPAWNING TASK");
        SpawnTask(GAME_TASK.GHOSTBUSTING, ROOM.LIVING_ROOM);
    }

    void SpawnTask(GAME_TASK task, ROOM room)
    {
        // EVENT_BUS.Publish(EventType.EVENT_STARTED, task, room);
        // ^ Would be soooo nice
        SetTaskSpawnPointForRoom(room);
        switch (task)
        {
            case GAME_TASK.GHOSTBUSTING:
                SpawnGhostbustingTask();
                break;
            case GAME_TASK.NUKE_DIFFUSING:
                break;
            case GAME_TASK.TOILET_PLUNGING:
                break;
            case GAME_TASK.DISPELLING_DARKNESS:
                break;
            case GAME_TASK.FEEDING_BOB:
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
        }
    }

    void SpawnGhostbustingTask()
    {
        // Amount of ghosts to spawn can be changed here
        int numGhosts = 25;

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

        foreach (GameObject ghost in ghosts)
        {
            ghost.SetActive(true);
        }
    }
}
