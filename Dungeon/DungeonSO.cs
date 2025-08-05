using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "New Dungeon",menuName = "DungeonSystem/New Dungeon")]
public class DungeonSO : ScriptableObject
{
    public GameObject dungeonPrefab;
    public MobSpawner[] mobSpawners;
    public NavMeshData bakedNavMesh;
    public int id = -1;

    public GameObject CreateDungeon()
    {
        GameObject dungeonInstance = Instantiate(dungeonPrefab, Vector3.zero, Quaternion.identity);
        dungeonInstance.GetComponent<Dungeon>().dungeonObject = this;
        return dungeonInstance;
    }

    public void ReviveAllMonster()
    {
        foreach (MobSpawner mobSpawner in mobSpawners)
        {
            mobSpawner.canSpawn = true;
        }
    }
}
