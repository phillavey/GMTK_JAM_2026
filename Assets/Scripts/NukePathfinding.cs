using UnityEngine;
using UnityEngine.AI;

public class NukePathfinding : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform playerTarget;

    [Header("Flee Settings")]
    [Tooltip("Distance at which the character notices the player and runs.")]
    [SerializeField] private float detectionRadius = 10f;
    [Tooltip("Distance the character tries to maintain/run to when fleeing.")]
    [SerializeField] private float fleeDistance =11f;

    [Header("Wander Settings")]
    [Tooltip("How far the character can wander from its current spot.")]
    [SerializeField] private float wanderRadius = 4f;
    [Tooltip("Time in seconds to wait at a wander point before picking a new one.")]
    [SerializeField] private float wanderWaitTime = 2f;

    private NavMeshAgent agent;
    private float wanderTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        wanderTimer = wanderWaitTime;
    }

    void Update()
    {
        if (!agent.isActiveAndEnabled || !agent.isOnNavMesh || playerTarget == null)
            return;

        WanderAimlessly();
    }

    private void FleeFromPlayer()
    {
        wanderTimer = wanderWaitTime;
        Vector3 fleeDirection = (transform.position - playerTarget.position).normalized;
        Vector3 rawFleeGoal = transform.position + fleeDirection * fleeDistance;

        if (NavMesh.SamplePosition(rawFleeGoal, out NavMeshHit hit, fleeDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    private void WanderAimlessly()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            wanderTimer += Time.deltaTime;

            if (wanderTimer >= wanderWaitTime)
            {
                Vector3 newWanderPoint = GetRandomWanderPoint();
                agent.SetDestination(newWanderPoint);

                wanderTimer = 0f;
            }
        }
    }

    private Vector3 GetRandomWanderPoint()
    {
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        Vector3 randomDirection = new Vector3(randomCircle.x, randomCircle.y, 0f);
        Vector3 finalPosition = transform.position + randomDirection;

        if (NavMesh.SamplePosition(finalPosition, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}
