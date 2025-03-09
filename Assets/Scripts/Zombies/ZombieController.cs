using UnityEngine;
using UnityEngine.AI;

public enum ZombieState
{
    Idle,
    Chase,
    Attack,
    SpecialSkill
}

public abstract class ZombieController : MonoBehaviour
{
    protected NavMeshAgent navMeshAgent;
    protected Transform player;
    protected ZombieState currentState;

    [Header("Zombie Settings")]
    public float detectionRange = 10f;
    public float chaseSpeed = 3.5f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;
    protected float lastAttackTime;

    protected virtual void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = chaseSpeed;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentState = ZombieState.Idle;
    }

    protected virtual void Update()
    {
        switch (currentState)
        {
            case ZombieState.Idle:
                UpdateIdle();
                break;
            case ZombieState.Chase:
                UpdateChase();
                break;
            case ZombieState.Attack:
                UpdateAttack();
                break;
            case ZombieState.SpecialSkill:
                UpdateSpecialSkill();
                break;
        }
    }

    protected void UpdateIdle()
    {
        if (player == null) return;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            currentState = ZombieState.Chase;
        }
    }

    protected virtual void UpdateChase()
    {
        if (player == null) return;
        navMeshAgent.SetDestination(player.position);

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            currentState = ZombieState.Attack;
            navMeshAgent.isStopped = true;
        }
        else if (distanceToPlayer > detectionRange)
        {
            currentState = ZombieState.Idle;
            navMeshAgent.isStopped = false;
        }
    }

    protected virtual void UpdateAttack()
    {
        if (player == null) return;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > attackRange)
        {
            currentState = ZombieState.Chase;
            navMeshAgent.isStopped = false;
            return;
        }

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            AttackPlayer();
            lastAttackTime = Time.time;
        }
    }

    protected virtual void AttackPlayer()
    {
    }

    protected virtual void UpdateSpecialSkill()
    {
    }
}