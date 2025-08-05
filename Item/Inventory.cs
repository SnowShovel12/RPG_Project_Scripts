using System;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;

[Serializable]
public class Inventory
{
    public InventorySlot[] slots = new InventorySlot[24];

    public void Clear()
    {
        foreach (InventorySlot slot in slots)
        {
            slot.RemoveItem();
        }
    }

    public bool IsContain(ItemSO itemObject)
    {
        return IsContain(itemObject.data.id);
    }

    public bool IsContain(int id)
    {
        return slots.FirstOrDefault(i => i.item.id == id) != null;
    }
}
