using UnityEngine;
using System.Collections.Generic;

public class MakeDungeonPlatform : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject[] wallPrefabs;
    public Vector2Int gridSize = new Vector2Int(7, 7);
    public float tileSpace = 4f;
    private Vector2 _center;

    [SerializeField,HideInInspector]
    private GameObject tiles = null;
    [SerializeField,HideInInspector]
    private GameObject walls = null;
    [SerializeField,HideInInspector]
    private GameObject cameraCollider = null;

#if UNITY_EDITOR
    public void GenerateDungeonTiles()
    {
        _center.x = (gridSize.x - 1) / 2f;
        _center.y = (gridSize.y - 1) / 2f;

        tiles = new GameObject("Tiles");
        tiles.transform.parent = transform;

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 spawnPoint = new Vector3((x - _center.x) * tileSpace, 0, (y - _center.y) * tileSpace);
                Instantiate(tilePrefab, spawnPoint, Quaternion.identity, tiles.transform);
            }
        }
    }

    public void GenerateDungeonWalls()
    {
        _center.x = (gridSize.x - 1) / 2f;
        _center.y = (gridSize.y - 1) / 2f;

        walls = new GameObject("Walls");
        walls.transform.parent = transform;

        for (int x = 0; x < gridSize.x; x++)
        {
            Vector3 spawnPoint1 = new Vector3((x - _center.x) * tileSpace, 0, - _center.y * tileSpace);
            Vector3 spawnPoint2 = new Vector3((x - _center.x) * tileSpace, 0, _center.y * tileSpace);
            Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)], spawnPoint1, Quaternion.identity, walls.transform);
            Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)], spawnPoint2, Quaternion.Euler(0, 180, 0), walls.transform);
        }
        for (int y = 0; y < gridSize.y; y++)
        {
            Vector3 spawnPoint1 = new Vector3(- _center.x * tileSpace, 0, (y - _center.y) * tileSpace);
            Vector3 spawnPoint2 = new Vector3(_center.x * tileSpace, 0, (y - _center.y) * tileSpace);
            Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)], spawnPoint1, Quaternion.Euler(0, 90, 0), walls.transform);
            Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)], spawnPoint2, Quaternion.Euler(0, 270, 0), walls.transform);
        }
    }

    public void GenerateCameraCollider()
    {
        Dungeon dungeon = GetComponent<Dungeon>();
        if (dungeon == null)
        {
            Debug.LogWarning("Add \"Dungeon\" component first");
            return;
        }
        cameraCollider = new GameObject("CameraCollider", typeof(BoxCollider));
        cameraCollider.transform.parent = transform;
        int limitedX = gridSize.x > 3 ? gridSize.x - 2 : gridSize.x;

        BoxCollider collider = cameraCollider.GetComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.size = new Vector3(limitedX * tileSpace, 20, gridSize.y * tileSpace);
        collider.center = new Vector3(0, 10, 0);
        dungeon.cameraConfinerVolume = collider;
    }

    public void RemoveDungeonTiles()
    {
        if (tiles)
        {
            DestroyImmediate(tiles);
        }
    }

    public void RemoveDungeonWalls()
    {
        if (walls)
        {
            DestroyImmediate(walls);
        }
    }

    public void RemoveCameraCollider()
    {
        if (cameraCollider)
        {
            DestroyImmediate(cameraCollider);
        }
    }
#endif
}
