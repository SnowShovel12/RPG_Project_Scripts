using System;
using UnityEditor.Rendering;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public ItemType[] allowedItems = new ItemType[0];

    [NonSerialized]
    public InventorySO parent;
    [NonSerialized]
    public GameObject slotUI;

    public event Action<InventorySlot> OnPreUpdate;
    public event Action<InventorySlot> OnPostUpdate;

    public Item item;
    public int amount;

    public ItemSO ItemObject
    {
        get
        {
            return item.id >= 0 ? parent.database.itemObjects[item.id] : null;
        }
    }

    public InventorySlot() => UpdateSlot(new Item(), 0);
    public InventorySlot(Item item, int amount) => UpdateSlot(item, amount);

    public void RemoveItem() => UpdateSlot(new Item(), 0);
    public void AddItem(Item item, int amount) => UpdateSlot(item, amount);

    public void AddAmount(int value) => UpdateSlot(item, amount += value);

    public void UpdateSlot(Item item, int amount)
    {
        OnPreUpdate?.Invoke(this);
        this.item = item;
        this.amount = amount;
        OnPostUpdate?.Invoke(this);
    }

    public bool CanPlaceInSlot(ItemSO itemObject)
    {
        if (allowedItems.Length <= 0 || itemObject == null || itemObject.data.id < 0)
        {
            return true;
        }

        foreach (ItemType type in allowedItems)
        {
            if (itemObject.type == type)
            {
                return true;
            }
        }

        return false;
    }
}
