using UnityEngine;
using System.Collections.Generic;

public class ObjectPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class PoolEntry
    {
        public GameObject prefab;
        public int initialSize = 20;
        [HideInInspector] public List<GameObject> pool;
    }

    [SerializeField] private List<PoolEntry> pools = new List<PoolEntry>();
    private Dictionary<GameObject, PoolEntry> poolDictionary = new Dictionary<GameObject, PoolEntry>();

    void Awake()
    {
        foreach (PoolEntry entry in pools)
        {
            entry.pool = new List<GameObject>();
            for (int i = 0; i < entry.initialSize; i++)
            {
                GameObject obj = Instantiate(entry.prefab);
                obj.SetActive(false);
                entry.pool.Add(obj);
            }
            poolDictionary[entry.prefab] = entry;
        }
    }

    public GameObject GetObject(GameObject prefab)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            return null;
        }

        PoolEntry poolEntry = poolDictionary[prefab];
        foreach (GameObject obj in poolEntry.pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        GameObject newObj = Instantiate(prefab);
        newObj.SetActive(true);
        poolEntry.pool.Add(newObj);
        return newObj;
    }

    public void ReturnObject(GameObject obj, GameObject prefab)
    {
        if (poolDictionary.ContainsKey(prefab))
        {
            obj.SetActive(false);
        }
    }
}