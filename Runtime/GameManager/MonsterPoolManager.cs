using UnityEngine;

public class MonsterPoolManager : PoolManager
{
    [SerializeField]
    private MonsterDBSO monsterDB;

    private void OnValidate()
    {
        MakePrefabs();
    }

    private void MakePrefabs()
    {
        if (monsterDB == null) return;
        prefabs = new GameObject[monsterDB.monsters.Length];
        for (int i = 0; i < monsterDB.monsters.Length; i++)
        {
            prefabs[i] = monsterDB.monsters[i].monsterPrefab;
        }
    }
}
