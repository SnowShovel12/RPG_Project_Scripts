using UnityEngine;

public interface IAttackable
{
    public int Damage { get; }
    public bool IsPlayer { get; }
    public bool IsAttacking { get; set; }
    public Transform Transform { get; }
    public Transform Target { get; }
}
