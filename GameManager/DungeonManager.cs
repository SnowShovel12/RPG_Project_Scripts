using Unity.AI.Navigation;
using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Unity.Cinemachine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance { get; private set; }

    public PlayerController player;
    private GameObject _currentDungeonInstance;
    private NavMeshDataInstance _currentNavMesh;
    public ScreenFader screenFader;
    [SerializeField]
    private CinemachineConfiner3D cameraConfiner;
    [SerializeField]
    private DungeonDBSO dungeonDB;
    public Vector3 initialSpawnPosition;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        dungeonDB.ReviveAllMonsters();

        LoadDungeonWithFade(dungeonDB.dungeons[0], initialSpawnPosition);
    }

    public void LoadDungeonWithFade(DungeonSO dungeon, Vector3 spawnPosition)
    {
        StartCoroutine(LoadDungeonRoutine(dungeon, spawnPosition));
    }

    private IEnumerator LoadDungeonRoutine(DungeonSO dungeon, Vector3 spawnPosition)
    {
        yield return screenFader.FadeOut();

        if (_currentDungeonInstance != null)
        {
            Destroy(_currentDungeonInstance);
        }

        GameManager.Instance.monsterPoolManager.DisableAllObjects();
        GameManager.Instance.hitboxPoolManager.DisableAllObjects();

        if (_currentNavMesh.valid)
        {
            _currentNavMesh.Remove();
        }

        _currentDungeonInstance = dungeon.CreateDungeon();

        if (dungeon.bakedNavMesh != null)
        {
            _currentNavMesh = NavMesh.AddNavMeshData(dungeon.bakedNavMesh, _currentDungeonInstance.transform.position, _currentDungeonInstance.transform.rotation);
        }

        player.WarpTo(spawnPosition);

        yield return screenFader.FadeIn();
    }

    public void SetCameraConfiner(Collider newConfiner)
    {
        if (cameraConfiner != null)
        {
            cameraConfiner.BoundingVolume = newConfiner;
        }
    }
}
