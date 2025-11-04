using System;
using UnityEngine;

[Serializable]
public abstract class MonsterAttack : Attack
{
    public int priority;
    public float range;
    [HideInInspector]
    public Monster monster;
}
