using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Patrolling,
    Investigating,
    Attacking
}

public class EnemyAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float attackRadius = 5f;
    public float shootCooldown = 1f;
    public Transform weapon;

    private NavMeshAgent agent;
    private Vector3 noisePosition;
    private int currentPatrolIndex = 0;
    private float lastShotTime = -Mathf.Infinity;
    private EnemyState currentState = EnemyState.Patrolling;
    public AudioSource audios;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        MicVolumeDetector.OnNoiseHeard += MoveToNoise;

        if (patrolPoints.Length > 0)
            agent.SetDestination(patrolPoints[0].position);
    }

    void OnDestroy()
    {
        MicVolumeDetector.OnNoiseHeard -= MoveToNoise;
    }

    void MoveToNoise(Vector3 position)
    {
        noisePosition = position;
        agent.SetDestination(noisePosition);
        SetState(EnemyState.Investigating);
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, noisePosition);

        if (MicVolumeDetector.isMakingNoise && distance <= attackRadius)
        {
            SetState(EnemyState.Attacking);
        }

        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
                break;
            case EnemyState.Investigating:
                Investigate();
                break;
            case EnemyState.Attacking:
                Attack();
                break;
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    void Investigate()
    {
        float distance = Vector3.Distance(transform.position, noisePosition);

        if (!MicVolumeDetector.isMakingNoise && distance <= attackRadius)
        {
            ReturnToPatrol();
        }
    }

    void Attack()
    {
        float distance = Vector3.Distance(transform.position, noisePosition);

        //Debug.Log("🔍 [EnemyAI] Detectando ruido: " + MicVolumeDetector.isMakingNoise);

        if (MicVolumeDetector.isMakingNoise && distance <= attackRadius)
        {
            TryShoot();
        }
        else
        {
            ReturnToPatrol();
        }
    }

    void TryShoot()
    {
        if (Time.time - lastShotTime >= shootCooldown)
        {
            lastShotTime = Time.time;
            Debug.Log("Disparo del enemigo");
            audios.Play();

        }
    }

    void ReturnToPatrol()
    {
        SetState(EnemyState.Patrolling);
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    void SetState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
            //Debug.Log("🧠 Cambio de estado a: " + currentState);
        }
    }
}
