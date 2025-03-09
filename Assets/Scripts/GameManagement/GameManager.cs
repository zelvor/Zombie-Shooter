using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public List<Room> rooms;
    public GameObject keyPrefab;
    private List<bool> hasKeys;
    public Transform outsideKeySpawnPoint;
    public Transform exitPoint;
    private HealthModule playerHealthModule;

    void Start()
    {
        hasKeys = new List<bool>(new bool[rooms.Count]);

        for (int i = 0; i < rooms.Count; i++)
        {
            rooms[i].SetRoomId(i, rooms.Count);
        }

        GameObject key = Instantiate(keyPrefab, outsideKeySpawnPoint.position, Quaternion.identity);
        key.GetComponent<Key>().SetKeyId(0);

        for (int i = 0; i < rooms.Count - 1; i++)
        {
            GameObject spawnedKey = rooms[i].SpawnKey(keyPrefab);
            spawnedKey.GetComponent<Key>().SetKeyId(i + 1);
        }

        // Tìm player và đăng ký sự kiện onDeath
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealthModule = player.GetComponent<HealthModule>();
            if (playerHealthModule != null)
            {
                playerHealthModule.onDeath.AddListener(OnPlayerDeath);
            }
        }
    }

    public void KeyCollected(int keyId)
    {
        hasKeys[keyId] = true;
    }

    public bool HasKey(int keyId)
    {
        return keyId >= 0 && keyId < hasKeys.Count && hasKeys[keyId];
    }

    public void PlayerReachedExit()
    {
        GameOverData.SetResult(true); // Victory
        SceneManager.LoadScene("GameOverScene");
    }

    private void OnPlayerDeath()
    {
        GameOverData.SetResult(false); // Lose
        SceneManager.LoadScene("GameOverScene");
    }
}

public static class GameOverData
{
    private static bool isVictory;

    public static void SetResult(bool victory)
    {
        isVictory = victory;
    }

    public static bool IsVictory()
    {
        return isVictory;
    }

    public static void Reset()
    {
        isVictory = false;
    }
}