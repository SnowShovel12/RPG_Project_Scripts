using System.Collections.Generic;
using UnityEngine;

public enum ItemType : int
{
    Helmet = 0,
    Chest = 1,
    Pants = 2,
    Weapon = 3,
    Food,
    Potion,
    Default
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Items/New Item")]
public class ItemSO : ScriptableObject
{
    public ItemType type;
    public bool stackable;

    public Sprite icon;
    public GameObject dropPrefab;

    public Item data = new Item();

    [TextArea(15, 20)]
    public string description;

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
}