using UnityEngine;

public class ExitPoint : MonoBehaviour
{
    public GameManager gameManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.PlayerReachedExit();
        }
    }
}