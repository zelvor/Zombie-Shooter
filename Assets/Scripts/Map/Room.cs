using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    public List<GameObject> zombies;
    public Transform keySpawnPoint;
    public Door door;
    private int roomId;
    private int totalRooms;

    void Start()
    {
        foreach (GameObject zombie in zombies)
        {
            zombie.SetActive(false);
        }
    }

    public void SetRoomId(int id, int totalRoomCount)
    {
        roomId = id;
        totalRooms = totalRoomCount;
    }

    public int GetRoomId()
    {
        return roomId;
    }

    public void OpenDoor()
    {
        foreach (GameObject zombie in zombies)
        {
            zombie.SetActive(true);
        }
    }

    public GameObject SpawnKey(GameObject keyPrefab)
    {
        if (keyPrefab != null && keySpawnPoint != null)
        {
            return Instantiate(keyPrefab, keySpawnPoint.position, Quaternion.identity, transform);
        }
        return null;
    }
}