using UnityEditor.Rendering;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
public class ItemDBSO : ScriptableObject
{
    public ItemSO[] itemObjects;

    public void OnValidate()
    {
        for (int i = 0; i < itemObjects.Length; i++)
        {
            itemObjects[i].data.id = i;
        }
    }
}