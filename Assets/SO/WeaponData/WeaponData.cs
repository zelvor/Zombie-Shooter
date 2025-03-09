using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Weapons/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("General Settings")]
    public string weaponName;
    public float fireRate = 1f;
    public float damage = 10f;
    public float reloadDuration = 1f;

    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public bool isAOE = false;
    public float aoeRadius = 0f;
    public float maxRange = 15f;
}