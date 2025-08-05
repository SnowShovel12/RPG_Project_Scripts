using System;
using UnityEngine;

[Serializable]
public class MonsterSkill : MonsterAttack
{
    [SerializeField]
    private MonsterSkillSO skillData;

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
        monster.animator.SetBool(_attackHash, true);
        monster.animator.SetInteger(_attackIndexHash, animationIndex);
    }

    public override void OnEnterAttack(IAttackable attacker, Transform target = null)
    {
        skillData?.Execute(monster, _direction, OnExitAttack);
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
