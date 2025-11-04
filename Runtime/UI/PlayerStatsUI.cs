using System;
using TMPro;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    public InventorySO equipment;
    public PlayerStatSO playerStats;

    public TextMeshProUGUI[] attributeText;

    private void OnEnable()
    {
        playerStats.OnChangedStat += OnChangedStats;

        if (equipment != null && playerStats != null)
        {
            foreach (InventorySlot slot in equipment.Slots)
            {
                slot.OnPreUpdate += OnRemoveItem;
                slot.OnPostUpdate += OnEquipItem;
            }
        }

        UpdateAttributeTexts();
    }

    private void OnDisable()
    {
        playerStats.OnChangedStat -= OnChangedStats;

        if (equipment != null && playerStats != null)
        {
            foreach (InventorySlot slot in equipment.Slots)
            {
                slot.OnPreUpdate -= OnRemoveItem;
                slot.OnPostUpdate -= OnEquipItem;
            }
        }
    }

    private void UpdateAttributeTexts()
    {
        attributeText[0].text = playerStats.GetModifiedValue(StatType.Health).ToString("n0");
        attributeText[1].text = playerStats.GetModifiedValue(StatType.Stamina).ToString("n0");
        attributeText[2].text = playerStats.GetModifiedValue(StatType.Strengh).ToString("n0");
        attributeText[3].text = playerStats.GetModifiedValue(StatType.Defense).ToString("n0");
    }

    private void OnRemoveItem(InventorySlot slot)
    {
        if (slot.ItemObject == null)
        {
            return;
        }

        float healthPercent = 0f;
        float staminaPercent = 0f;

        foreach (Modifier buff in slot.item.buffs)
        {
            foreach (ModifiableValue stat in playerStats.stats)
            {
                if (stat.Type == buff.statType)
                {
                    if (buff.statType == StatType.Health) { healthPercent = playerStats.HealthPercent; }
                    else if (buff.statType == StatType.Stamina) { staminaPercent = playerStats.StaminaPercent; }

                    stat.RemoveModifier(buff);

                    if (buff.statType == StatType.Health) { playerStats.Health = (int)Math.Round(healthPercent * playerStats.GetModifiedValue(StatType.Health)); }
                    else if (buff.statType == StatType.Stamina) { playerStats.Stamina = (int)Math.Round(staminaPercent * playerStats.GetModifiedValue(StatType.Stamina)); }
                }
            }
        }

        playerStats.OnChangedStat?.Invoke(playerStats);
    }

    private void OnEquipItem(InventorySlot slot)
    {
        if (slot.ItemObject == null)
        {
            return;
        }

        float healthPercent = 0f;
        float staminaPercent = 0f;

        foreach (Modifier buff in slot.item.buffs)
        {
            foreach (ModifiableValue stat in playerStats.stats)
            {
                if (stat.Type == buff.statType)
                {
                    if (buff.statType == StatType.Health) { healthPercent = playerStats.HealthPercent; }
                    else if (buff.statType == StatType.Stamina) { staminaPercent = playerStats.StaminaPercent; }

                    stat.AddModifier(buff);

                    if (buff.statType == StatType.Health) { playerStats.Health = (int)Math.Round(healthPercent * playerStats.GetModifiedValue(StatType.Health)); }
                    else if (buff.statType == StatType.Stamina) { playerStats.Stamina = (int)Math.Round(staminaPercent * playerStats.GetModifiedValue(StatType.Stamina)); }
                }
            }
        }

        playerStats.OnChangedStat?.Invoke(playerStats);
    }

    private void OnChangedStats(PlayerStatSO statsObject)
    {
        UpdateAttributeTexts();
    }
}

