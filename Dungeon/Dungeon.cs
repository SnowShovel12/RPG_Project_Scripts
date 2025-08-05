using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public DungeonSO dungeonObject;
    public BoxCollider cameraConfinerVolume;

    private void Start()
    {
        foreach (MobSpawner mobSpawner in dungeonObject.mobSpawners)
        {
            mobSpawner.SpawnMonster(this);
        }

        if (DungeonManager.Instance && cameraConfinerVolume)
        {
            DungeonManager.Instance.SetCameraConfiner(cameraConfinerVolume);
        }
    }
}
