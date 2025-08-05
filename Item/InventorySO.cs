using UnityEditor.Experimental.GraphView;
using System.Linq;
using UnityEngine;
using System;
using UnityEditor.Rendering;

public enum InterfaceType
{
    Inventory,
    Equipment,
    QuickSlot,
    Chest,
    Trash,
}

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventorySO : ScriptableObject
{
    public ItemDBSO database;
    public InterfaceType type;

    [SerializeField]
    private Inventory container = new Inventory();

    public Action<ItemSO> OnUseItem;

    public InventorySlot[] Slots => container.slots;

    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            foreach (InventorySlot slot in Slots)
            {
                if (slot.item.id <= 0) counter++;
            }

            return counter;
        }
    }

    public bool AddItem(Item item, int amount)
    {
        if (EmptySlotCount <= 0)
        {
            return false;
        }

        InventorySlot slot = FindItemInInventory(item);
        if (!database.itemObjects[item.id].stackable || slot == null)
        {
            GetEmptySlot().AddItem(item, amount);
        }
        else
        {
            slot.AddAmount(amount);
        }

        return true;
    }

    public InventorySlot FindItemInInventory(Item item)
    {
        return Slots.FirstOrDefault(i => i.item.id == item.id);
    }

    public InventorySlot GetEmptySlot()
    {
        return Slots.FirstOrDefault(i => i.item.id < 0);
    }

    public bool IsContain(ItemSO itemObject)
    {
        return container.IsContain(itemObject);
    }

    public void SwapItems(InventorySlot itemSlotA, InventorySlot itemSlotB)
    {
        if (itemSlotA == itemSlotB)
        {
            return;
        }

        if (itemSlotB.CanPlaceInSlot(itemSlotA.ItemObject) && itemSlotA.CanPlaceInSlot(itemSlotB.ItemObject))
        {
            InventorySlot tempSlot = new InventorySlot(itemSlotB.item, itemSlotB.amount);
            itemSlotB.UpdateSlot(itemSlotA.item, itemSlotA.amount);
            itemSlotA.UpdateSlot(tempSlot.item, tempSlot.amount);
        }
    }

    public void ClearInventory()
    {
        container.Clear();
    }

    public void UseItem(InventorySlot slot)
    {
        if (slot.ItemObject == null || slot.item.id < 0 || slot.amount <= 0)
        {
            return;
        }

        ItemSO itemObject = slot.ItemObject;
        slot.UpdateSlot(slot.item, slot.amount - 1);

        OnUseItem.Invoke(itemObject);
    }

    public void InitializeSlots()
    {
        foreach (InventorySlot slot in Slots)
        {
            slot.parent = this;
        }
    }
}

