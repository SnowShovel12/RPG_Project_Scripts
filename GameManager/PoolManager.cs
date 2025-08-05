using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;

    private List<GameObject>[] _pools;

    private void Awake()
    {
        _pools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < _pools.Length; i++)
        {
            _pools[i] = new List<GameObject>();
        }
    }

    public T Get<T>(int id) where T : Component
    {
        List<int> matchingIndex = new List<int>();
        for (int i = 0; i < prefabs.Length; i++)
        {
            if (prefabs[i].GetComponent<T>() != null)
            {
                matchingIndex.Add(i);
            }
        }

        if (id < 0 || id >= matchingIndex.Count)
        {
            return null;
        }
        int prefabIndex = matchingIndex[id];

        foreach (GameObject go in _pools[prefabIndex])
        {
            if (!go.activeSelf)
            {
                go.SetActive(true);
                return go.GetComponent<T>();
            }
        }

        GameObject instance = Instantiate(prefabs[prefabIndex], transform);
        _pools[prefabIndex].Add(instance);
        return instance.GetComponent<T>();
    }

    public void DisableAllObjects()
    {
        foreach(Transform ob in transform)
        {
            ob.gameObject.SetActive(false);
        }
    }
}
