using UnityEngine;

public class TrashCanInventoryUI : StaticInventoryUI
{
    private void Update()
    {
        if (inventoryObject.Slots[0].item.id == -1) return;
        inventoryObject.ClearInventory();
    }
}
