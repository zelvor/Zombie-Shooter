using UnityEngine;
using UnityEngine.AI;

public class RangeZombie : ZombieController, IWeaponUser
{
    [Header("Range Zombie Settings")]
    public float minDistanceToPlayer = 5f;
    public float gunDamage = 5f;
    public float gunRange = 8f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Weapon weapon;
    [SerializeField] private WeaponData weaponData;

    protected override void Start()
    {
        base.Start();
        attackRange = gunRange;
        weaponData.damage = gunDamage;
        weaponData.maxRange = gunRange;
        weaponData.weaponName = "Assault Rifle";
        weapon.SetWeaponData(weaponData);
    }

    protected override void UpdateChase()
    {
        if (player == null) return;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= minDistanceToPlayer)
        {
            Vector3 directionAway = (transform.position - player.position).normalized;
            Vector3 targetPosition = transform.position + directionAway * (minDistanceToPlayer - distanceToPlayer);
            navMeshAgent.SetDestination(targetPosition);
        }
        else if (distanceToPlayer <= attackRange)
        {
            currentState = ZombieState.Attack;
            navMeshAgent.isStopped = true;
        }
        else if (distanceToPlayer > detectionRange)
        {
            currentState = ZombieState.Idle;
            navMeshAgent.isStopped = false;
        }
        else
        {
            navMeshAgent.SetDestination(player.position);
        }

        if (distanceToPlayer <= attackRange)
        {
            Vector3 lookDirection = (player.position - transform.position).normalized;
            lookDirection.y = 0;
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }

    protected override void UpdateAttack()
    {
        base.UpdateAttack();
        if (player == null) return;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= gunRange)
        {
            Vector3 lookDirection = (player.position - transform.position).normalized;
            lookDirection.y = 0;
            transform.rotation = Quaternion.LookRotation(lookDirection);
            weapon?.ManualUpdate();
        }
        else
        {
            currentState = ZombieState.Chase;
            navMeshAgent.isStopped = false;
        }
    }

    public bool ShouldFire()
    {
        return weapon != null && Vector3.Distance(transform.position, player.position) <= gunRange;
    }
}