using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float damage;
    private float speed;
    private bool isAOE;
    private float aoeRadius;
    private Vector3 velocity;
    private ObjectPoolManager poolManager;
    private GameObject prefab;
    private Vector3 targetPosition;
    private float maxRange;
    private Vector3 startPosition;

    void Update()
    {
        if (isAOE)
        {
            velocity.y += Physics.gravity.y * Time.deltaTime;
            Vector3 nextPosition = transform.position + velocity * Time.deltaTime;
            if (!HandleCollision(nextPosition, velocity.normalized))
            {
                transform.position = nextPosition;
            }

            if (transform.position.y <= targetPosition.y || Vector3.Distance(startPosition, transform.position) > maxRange)
            {
                Explode();
            }
        }
        else
        {
            Vector3 moveDirection = transform.forward;
            float moveDistance = speed * Time.deltaTime;
            Vector3 nextPosition = transform.position + moveDirection * moveDistance;
            if (!HandleCollision(nextPosition, moveDirection))
            {
                transform.position = nextPosition;
            }

            if (transform.position.y < -10f)
            {
                poolManager.ReturnObject(gameObject, prefab);
            }
        }
    }

    public void Initialize(float damage, float speed, bool isAOE, float aoeRadius, ObjectPoolManager poolManager, GameObject prefab, Vector3 targetPosition, float maxRange)
    {
        this.damage = damage;
        this.speed = speed;
        this.isAOE = isAOE;
        this.aoeRadius = aoeRadius;
        this.poolManager = poolManager;
        this.prefab = prefab;
        this.targetPosition = targetPosition;
        this.maxRange = maxRange;
        this.startPosition = transform.position;

        if (isAOE)
        {
            CalculateParabolicVelocity();
        }
    }

    void CalculateParabolicVelocity()
    {
        Vector3 toTarget = targetPosition - transform.position;
        float distance = new Vector3(toTarget.x, 0, toTarget.z).magnitude;
        float height = toTarget.y;

        float timeToTarget = distance / speed;
        Vector3 horizontalVelocity = new Vector3(toTarget.x, 0, toTarget.z).normalized * speed;
        float verticalVelocity = (height - 0.5f * Physics.gravity.y * timeToTarget * timeToTarget) / timeToTarget;

        velocity = horizontalVelocity + Vector3.up * verticalVelocity;
    }

    bool HandleCollision(Vector3 nextPosition, Vector3 direction)
    {
        Vector3 moveVector = nextPosition - transform.position;
        float moveDistance = moveVector.magnitude;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, moveDistance))
        {
            transform.position = hit.point;

            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }

            if (!isAOE)
            {
                poolManager.ReturnObject(gameObject, prefab);
            }
            return true;
        }
        return false;
    }

    void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, aoeRadius);
        foreach (Collider hit in hitColliders)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }
        poolManager.ReturnObject(gameObject, prefab);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isAOE)
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
                poolManager.ReturnObject(gameObject, prefab);
            }
        }
    }
}