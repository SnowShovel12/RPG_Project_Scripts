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

    private int _addValue;
    private int _multiplyValue;

    private int _modifiedValue;
    public int ModifiedValue => _modifiedValue;

    private List<Modifier> _modifiers = new List<Modifier>();

    public void AddModifier(Modifier modifier)
    {
        _modifiers.Add(modifier);
        ModifyValue();
    }

    public void RemoveModifier(Modifier modifier)
    {
        _modifiers.Remove(modifier); 
        ModifyValue();
    }

    public void ModifyValue()
    {
        _addValue = 0;
        _multiplyValue = 0;
        foreach (Modifier modifier in _modifiers)
        {
            if (modifier.statType == type)
            {
                if (modifier.type == ModifierType.Add) _addValue += modifier.value;
                else if (modifier.type == ModifierType.Multiply) _multiplyValue += modifier.value;
            }
        }
        _modifiedValue = (int)Math.Round((baseValue + _addValue) * (1 + _multiplyValue / 100f));
    }

    public void ClearModifier()
    {
        _modifiers.Clear();
    }
}
