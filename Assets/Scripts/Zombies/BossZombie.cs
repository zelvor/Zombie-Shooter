using UnityEngine;
using UnityEngine.AI;

public class BossZombie : ZombieController, IWeaponUser
{
    [Header("Boss Settings")]
    public float meleeDamage = 20f;
    public float gunDamage = 15f;
    public float gunRange = 8f;
    [SerializeField] private Transform firePoint;
    private Weapon weapon;
    [SerializeField] private WeaponData weaponData;

    [Header("AOE Explosion Settings")]
    public float aoeRadius = 5f;
    public float aoeDamage = 50f;
    public float aoeAnticipationTime = 3f;
    public GameObject aoeIndicatorPrefab;

    private float specialSkillCooldown = 10f;
    private float lastSpecialSkillTime;
    private bool isUsingSpecialSkill = false;
    private float specialSkillTimer;

    protected override void Start()
    {
        base.Start();
        attackRange = gunRange;
        lastSpecialSkillTime = -specialSkillCooldown;

        weapon = gameObject.AddComponent<Weapon>();

        weaponData.damage = gunDamage;
        weaponData.maxRange = gunRange;
        weaponData.weaponName = "Assault Rifle";
        weapon.SetWeaponData(weaponData);
    }

    protected override void UpdateChase()
    {
        if (Time.time - lastSpecialSkillTime >= specialSkillCooldown && !isUsingSpecialSkill)
        {
            currentState = ZombieState.SpecialSkill;
            return;
        }

        base.UpdateChase();

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > detectionRange * 0.5f)
        {
            navMeshAgent.speed = chaseSpeed * 0.7f;
        }
        else
        {
            navMeshAgent.speed = chaseSpeed;
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
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= 1.5f)
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                lastAttackTime = Time.time;
            }
        }
        else if (distanceToPlayer <= gunRange)
        {
            Vector3 lookDirection = (player.position - transform.position).normalized;
            lookDirection.y = 0;
            transform.rotation = Quaternion.LookRotation(lookDirection);

            if (weapon != null)
            {
                weapon.ManualUpdate();
            }
        }
        else
        {
            currentState = ZombieState.Chase;
            navMeshAgent.isStopped = false;
        }
    }

    protected override void UpdateSpecialSkill()
    {
        if (!isUsingSpecialSkill)
        {
            isUsingSpecialSkill = true;
            specialSkillTimer = aoeAnticipationTime;

            if (aoeIndicatorPrefab != null)
            {
                GameObject indicator = Instantiate(aoeIndicatorPrefab, transform.position, Quaternion.identity);
                indicator.transform.localScale = new Vector3(aoeRadius * 2, 0.1f, aoeRadius * 2);
                Destroy(indicator, aoeAnticipationTime);
            }
        }

        specialSkillTimer -= Time.deltaTime;
        if (specialSkillTimer <= 0)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, aoeRadius);
            foreach (Collider hit in hitColliders)
            {
                if (hit.CompareTag("Player"))
                {
                }
            }

            lastSpecialSkillTime = Time.time;
            isUsingSpecialSkill = false;
            currentState = ZombieState.Chase;
            navMeshAgent.isStopped = false;
        }
    }

    public bool ShouldFire()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        return distanceToPlayer <= gunRange && distanceToPlayer > 1.5f;
    }
}