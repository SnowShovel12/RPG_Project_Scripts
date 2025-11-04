using System;
using UnityEngine;

[Serializable]
public class PlayerMeleeAttack : PlayerAttack
{
    private Hitbox _hitbox;
    public HitboxConfig hitboxConfig;
    public StatusEffectSO statusEffect;
    public float effectDuration = 0;

    public override void OnInitialized()
    {
        base.OnInitialized();
    }

    public override void ExecuteAttack()
    {
        player.animator.SetBool(_attackHash, true);
        player.animator.SetInteger(_attackIndexHash, animationIndex);
        player.LookHandleDirection();
    }

    public override void OnEnterAttack(IAttackable attacker, Transform target = null)
    {
        _hitbox = GameManager.Instance.hitboxPoolManager.Get<Hitbox>(0);
        _hitbox.ActivateHitbox(GetDamage(attacker), attacker, hitboxConfig, statusEffect, effectDuration);
    }

    protected override int GetDamage(IAttackable attacker)
    {
        return (int)Math.Round(attacker.Damage * modifier);
    }

    public override void OnExitAttack()
    {
        _hitbox.RemoveHitbox();
        _hitbox = null;
    }
}
