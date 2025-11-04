using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Hitbox : MonoBehaviour
{
    private int _damage;
    private IAttackable _attacker;
    private StatusEffectSO _statusEffect;
    private float _effectDuration;
    private BoxCollider _collider;
    private HitboxConfig _config;

    private HashSet<IDamagable> damagedTargets = new HashSet<IDamagable> ();
    private void OnTriggerEnter(Collider other)
    {
        IDamagable target = other.GetComponent<IDamagable>();
        if (target == null || damagedTargets.Contains(target) || _attacker == target) return;
        
        bool canAttack = _attacker.IsPlayer || target.IsPlayer;

        if (canAttack && target.IsAlive)
        {
            target.TakeDamage(_damage, _attacker, _statusEffect, _effectDuration);
            damagedTargets.Add(target);
        }
    }

    private void Awake()
    {
        _collider = GetComponent<BoxCollider> ();
    }

    private void OnEnable()
    {
        damagedTargets.Clear();
    }

    private void LateUpdate()
    {
        if (_config != null && _config.followAttacker)
        {
            transform.position = _attacker.Transform.TransformPoint(_config.offset);
            transform.rotation = _attacker.Transform.rotation;
        }
    }

    public void ActivateHitbox(int damage, IAttackable attacker, HitboxConfig config, StatusEffectSO statusEffect, float effectDuration)
    {
        _damage = damage;
        _attacker = attacker;
        _statusEffect = statusEffect;
        _effectDuration = effectDuration;
        _config = config;

        _collider.size = config.size;
        transform.position = attacker.Transform.TransformPoint(config.offset);
        transform.rotation = attacker.Transform.rotation;

        gameObject.SetActive(true);
    }

    public void ActivateHitbox(int damage, IAttackable attacker, HitboxConfig config)
    {
        ActivateHitbox(damage, attacker, config, null, 0);
    }

    public void RemoveHitbox()
    {
        gameObject.SetActive(false);
    }
}
