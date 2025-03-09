using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private ObjectPoolManager poolManager;
    private WeaponData data;
    private float nextFireTime;
    private bool isAiming;
    private IWeaponUser weaponUser;

    private void Awake()
    {
        weaponUser = GetComponentInParent<IWeaponUser>();
    }

    public void SetWeaponData(WeaponData newData)
    {
        this.data = newData;
        nextFireTime = 0f;
        isAiming = false;
    }

    public void ManualUpdate()
    {
        if (data == null || weaponUser == null) return;

        if (data.weaponName == "Assault Rifle")
        {
            if (weaponUser.ShouldFire() && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / data.fireRate;
            }
        }
        else if (data.weaponName == "Grenade Launcher")
        {
            if (weaponUser.ShouldFire() && !isAiming)
            {
                isAiming = true;
            }
            if (!weaponUser.ShouldFire() && Time.time >= nextFireTime && isAiming)
            {
                Shoot();
                nextFireTime = Time.time + 1f / data.fireRate;
                isAiming = false;
            }
        }
    }

    private void Shoot()
    {
        if (data.projectilePrefab == null || poolManager == null) return;

        GameObject projectile = poolManager.GetObject(data.projectilePrefab);
        if (projectile == null) return;

        projectile.transform.position = firePoint.position;
        projectile.transform.rotation = firePoint.rotation;

        Projectile projScript = projectile.GetComponent<Projectile>();
        if (projScript != null)
        {
            if (data.isAOE)
            {
                Vector3 targetPosition = GetTargetPosition();
                projScript.Initialize(data.damage, data.projectileSpeed, data.isAOE, data.aoeRadius, poolManager, data.projectilePrefab, targetPosition, data.maxRange);
            }
            else
            {
                projScript.Initialize(data.damage, data.projectileSpeed, data.isAOE, data.aoeRadius, poolManager, data.projectilePrefab, Vector3.zero, 0f);
            }
        }
    }

    private Vector3 GetTargetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 direction = hit.point - firePoint.position;
            float distance = direction.magnitude;

            if (distance <= data.maxRange)
            {
                return hit.point;
            }
            else
            {
                direction.y = 0;
                direction.Normalize();
                return firePoint.position + direction * data.maxRange;
            }
        }

        Vector3 defaultDirection = firePoint.forward;
        defaultDirection.y = 0;
        defaultDirection.Normalize();
        return firePoint.position + defaultDirection * data.maxRange;
    }

    public float GetReloadDuration()
    {
        return data != null ? data.reloadDuration : 0f;
    }
}