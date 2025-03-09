using UnityEngine;
using UnityEngine.Events;

public class HealthModule : MonoBehaviour, IDamageable
{
    [SerializeField] private ScriptableObject healthData;
    private IHealthData healthDataProvider;
    private float currentHealth;
    public UnityEvent onDeath; // Sự kiện được gọi khi chết

    void Start()
    {
        healthDataProvider = healthData as IHealthData;
        if (healthDataProvider == null)
        {
            Debug.LogError($"{name}: HealthData does not implement IHealthData!");
            currentHealth = 100f;
        }
        else
        {
            currentHealth = healthDataProvider.GetHealth();
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void SetHealthData(ScriptableObject newHealthData)
    {
        healthData = newHealthData;
        healthDataProvider = healthData as IHealthData;
        if (healthDataProvider != null)
        {
            currentHealth = healthDataProvider.GetHealth();
        }
    }

    private void Die()
    {
        onDeath.Invoke();
        Destroy(gameObject);
    }
}