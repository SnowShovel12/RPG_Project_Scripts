using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;

    private List<GameObject>[] _pools;

    private List<int> _matchingIndex = new List<int>();

    private void Awake()
    {
        _pools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < _pools.Length; i++)
        {
            _pools[i] = new List<GameObject>();
        }
    }

    public T Get<T>(int id, Transform parent) where T : Component
    {
        _matchingIndex.Clear();
        for (int i = 0; i < prefabs.Length; i++)
        {
            if (prefabs[i].GetComponent<T>() != null)
            {
                _matchingIndex.Add(i);
            }
        }

        if (id < 0 || id >= _matchingIndex.Count)
        {
            return null;
        }
        int prefabIndex = _matchingIndex[id];

        foreach (GameObject go in _pools[prefabIndex])
        {
            if (!go.activeSelf)
            {
                go.SetActive(true);
                go.transform.SetParent(parent);
                return go.GetComponent<T>();
            }
        }

        GameObject instance = Instantiate(prefabs[prefabIndex], parent);
        _pools[prefabIndex].Add(instance);
        return instance.GetComponent<T>();
    }

    public T Get<T>(int id) where T : Component
    {
        return Get<T>(id, transform);
    }

    public void DisableAllObjects()
    {
        foreach(Transform ob in transform)
        {
            ob.gameObject.SetActive(false);
        }
    }
}
