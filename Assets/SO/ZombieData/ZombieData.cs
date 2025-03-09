using UnityEngine;

[CreateAssetMenu(fileName = "NewZombie", menuName = "Zombies/ZombieData")]
public class ZombieData : ScriptableObject, IHealthData
{
    public float hp;
    public float moveSpeed;

    public float GetHealth()
    {
        return hp;
    }
}