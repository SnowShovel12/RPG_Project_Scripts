using UnityEngine;

public interface IDamagable
{
    public bool IsAlive {  get; }
    public bool IsPlayer {  get; }
    public bool IsStunned { get; set; }
    public bool IsImmune { get; set; }
    public Animator Animator { get; }
    public void TakeDamage(int damage, IAttackable attacker, StatusEffectSO statusEffect = null, float duration = 0);
    public void ApplyStatusEffect(StatusEffectSO statusEffect = null, float duration = 0);
    public float GetHealthPercent();
}
