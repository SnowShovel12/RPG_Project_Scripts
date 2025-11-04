using System;
using System.IO;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class Item
{
    public int id = -1;
    public string itemName;

    public Modifier[] buffs;

    public Item()
    {
        id = -1;
        itemName = "";
    }

    public Item(ItemSO itemObject)
    {
        itemName = itemObject.name;
        id = itemObject.data.id;

        buffs = new Modifier[itemObject.data.buffs.Length];
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new Modifier()
            {
                statType = itemObject.data.buffs[i].statType,
                value = itemObject.data.buffs[i].value,
                type = itemObject.data.buffs[i].type
            };
        }
    }
}
