using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private ZombieData zombieData;
    private HealthModule healthModule;

    void Start()
    {
        healthModule = GetComponent<HealthModule>();
        healthModule.SetHealthData(zombieData);
    }
}