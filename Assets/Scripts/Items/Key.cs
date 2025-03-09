using UnityEngine;

public class Key : MonoBehaviour
{
    private GameManager gameManager;
    private int keyId;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameManager != null)
            {
                Debug.Log($"Key {keyId} picked up! Calling KeyCollected for Room {keyId + 1}.");
                gameManager.KeyCollected(keyId);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("GameManager is null in Key script!");
            }
        }
    }

    public void SetKeyId(int id)
    {
        keyId = id;
    }
}