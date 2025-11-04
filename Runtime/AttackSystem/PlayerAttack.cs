using System;
using UnityEngine;

[Serializable]
public abstract class PlayerAttack : Attack
{
    public int cost;
    [HideInInspector]
    public PlayerController player;
}
