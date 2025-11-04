using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MakeDungeonPlatform))]
public class MakeDungeonPlatformEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MakeDungeonPlatform makeDungeonPlatform = (MakeDungeonPlatform)target;

        if (GUILayout.Button("Generate Dungeon Tiles"))
        {
            makeDungeonPlatform.GenerateDungeonTiles();
        }

        if (GUILayout.Button("Generate Dungeon Walls"))
        {
            makeDungeonPlatform.GenerateDungeonWalls();
        }

        if (GUILayout.Button("Generate Camera Collider"))
        {
            makeDungeonPlatform.GenerateCameraCollider();
        }

        if (GUILayout.Button("Remove Dungeon Tiles"))
        {
            makeDungeonPlatform.RemoveDungeonTiles();
        }

        if (GUILayout.Button("Remove Dungeon Walls"))
        {
            makeDungeonPlatform.RemoveDungeonWalls();
        }

        if (GUILayout.Button("Remove Camera Collider"))
        {
            makeDungeonPlatform.RemoveCameraCollider();
        }
    }
}
