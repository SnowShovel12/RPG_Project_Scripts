using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ItemUI : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public TextMeshProUGUI[] itemBuffs;

    private void Update()
    {
        if (MouseData.slotHoveredOver != null)
        {
            InventorySlot slot = MouseData.slotHoveredOver;
            if (slot.item.id < 0)
            {
                ResetText();
                return;
            }
            itemName.text = slot.ItemObject.name;
            itemDescription.text = slot.ItemObject.description;
            for (int i = 0; i < slot.item.buffs.Length; i++)
            {
                if (i == 4) break;
                Modifier buff = slot.item.buffs[i];
                string modifierType = (buff.type == ModifierType.Multiply) ? "%" : "";
                string modifierValue = buff.value.ToString();
                string modifierStat = buff.statType.ToString();
                itemBuffs[i].text = modifierStat + " + " + modifierValue + modifierType;
            }
        }
    }

    private void ResetText()
    {
        itemName.text = "";
        itemDescription.text = "";
        foreach (TextMeshProUGUI itemBuff in itemBuffs)
        {
            itemBuff.text = "";
        }
    }
}
