using UnityEngine;

[CreateAssetMenu(fileName = "New DungeonDB", menuName = "DungeonSystem/New DungeonDB")]
public class DungeonDBSO : ScriptableObject
{
    public DungeonSO[] dungeons;

    private void OnValidate()
    {
        if (dungeons.Length != 0)
        {
            for (int i =0; i < dungeons.Length; i++)
            {
                dungeons[i].id = i;
            }
        }
    }

    public void ReviveAllMonsters()
    {
        foreach (DungeonSO dungeon in dungeons)
        {
            dungeon.ReviveAllMonster();
        }
    }
}
