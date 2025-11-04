using UnityEngine;
using System.Collections;

public class StatInitializer : MonoBehaviour
{
    public InventorySO equipment;
    public PlayerStatSO playerStats;

    private void Start()
    {
        equipment.InitializeSlots();
        ApplyStats();
    }

    private void ApplyStats()
    {
        foreach (InventorySlot slot in equipment.Slots)
        {
            ItemSO itemSO = slot.ItemObject;
            if (itemSO == null) continue;

            foreach (Modifier buff in slot.item.buffs)
            {
                foreach (ModifiableValue stat in playerStats.stats)
                {
                    if (stat.Type == buff.statType)
                        stat.AddModifier(buff);
                }
            }
        }
        playerStats.HealPlayer();

        playerStats.OnChangedStat?.Invoke(playerStats);
    }
}
