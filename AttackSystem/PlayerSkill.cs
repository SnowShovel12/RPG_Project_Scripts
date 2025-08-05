using System;
using UnityEngine;

[Serializable]
public class PlayerSkill : PlayerAttack
{
    [SerializeField]
    private PlayerSkillSO skillData;

    [SerializeField]
    private bool withHitbox;
    private Hitbox _hitbox;
    public HitboxConfig hitboxConfig;
    public AttackIndicatorConfig indicatorConfig;

    public override void OnInitialized()
    {
        base.OnInitialized();
    }

    public override void ExecuteAttack()
    {
        player.animator.SetBool(attackHash, true);
        player.animator.SetInteger(attackIndexHash, animationIndex);

        skillData?.Execute(player, direction, OnExitAttack);
    }

    public override void OnEnterAttack(IAttackable attacker, Transform target = null)
    {
        if (!withHitbox) return;
        _hitbox = GameManager.Instance.hitboxPoolManager.Get<Hitbox>(0);
        _hitbox.ActivateHitbox(GetDamage(attacker), attacker, hitboxConfig);
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
