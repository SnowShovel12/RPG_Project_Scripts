using System;
using UnityEngine;

public enum ModifierType
{
    Add,
    Multiply,
}

[Serializable]
public class Modifier
{
    public ModifierType type;

    public StatType statType;

    public int value;
}
