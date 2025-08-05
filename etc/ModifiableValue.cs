using System;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Health,
    Stamina,
    Strengh,
    Defense,
} 

[Serializable]
public class ModifiableValue
{
    [SerializeField]
    private StatType type;
    public StatType Type => type;
    public int baseValue;

    private int addValue;
    private int multiplyValue;

    private int modifiedValue;
    public int ModifiedValue => modifiedValue;

    private List<Modifier> modifiers = new List<Modifier>();

    public void AddModifier(Modifier modifier)
    {
        modifiers.Add(modifier);
        ModifyValue();
    }

    public void RemoveModifier(Modifier modifier)
    {
        modifiers.Remove(modifier); 
        ModifyValue();
    }

    public void ModifyValue()
    {
        addValue = 0;
        multiplyValue = 0;
        foreach (Modifier modifier in modifiers)
        {
            if (modifier.statType == type)
            {
                if (modifier.type == ModifierType.Add) addValue += modifier.value;
                else if (modifier.type == ModifierType.Multiply) multiplyValue += modifier.value;
            }
        }
        modifiedValue = (int)Math.Round((baseValue + addValue) * (1 + multiplyValue / 100f));
    }

    public void ClearModifier()
    {
        modifiers.Clear();
    }
}
