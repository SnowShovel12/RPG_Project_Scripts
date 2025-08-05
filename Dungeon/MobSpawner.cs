using System;
using UnityEngine;

public enum Direction
{
    Forward,
    Backward,
    Right,
    Left,
}

[Serializable]
public class MobSpawner
{
    public MonsterSO monsterObject;
    public bool canSpawn = true;
    public Vector2 spawnPoint = Vector2.zero;
    public Direction lookDirection = Direction.Forward;
    public DropTable dropTable;

    public void SpawnMonster(Dungeon dungeon)
    {
        if (!canSpawn) return;
        Vector3 spawnPoint3D = new Vector3(spawnPoint.x, 0, spawnPoint.y);
        //GameObject go = GameObject.Instantiate(monsterObject.monsterPrefab, spawnPoint3D, GetRotationFromDirection(lookDirection), dungeon.transform);
        Monster monster = GameManager.Instance.monsterPoolManager.Get<Monster>(monsterObject.id);
        monster.Set(this, spawnPoint3D, GetRotationFromDirection(lookDirection));
    }

    private Quaternion GetRotationFromDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.Forward: return Quaternion.identity;
            case Direction.Backward: return Quaternion.Euler(0, 180, 0);
            case Direction.Left: return Quaternion.Euler(0, -90, 0);
            case Direction.Right: return Quaternion.Euler(0, 90, 0);
            default: return Quaternion.identity;
        }
    }
}
