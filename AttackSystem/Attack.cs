using System;
using System.Collections;
using UnityEngine;

[Serializable]
public abstract class Attack
{
    public bool hasGlobalCoolTime;

    public float coolTime;
    protected float _currentCoolTime;
    public float CurrentCooltime => _currentCoolTime;
    public float CooltimeRatio => _currentCoolTime / coolTime;

    public bool IsReady => _currentCoolTime <= 0;

    public float modifier = 1f;

    public int animationIndex;

    protected readonly int _attackHash = Animator.StringToHash("Attack");
    protected readonly int _attackIndexHash = Animator.StringToHash("AttackIndex");

    protected Vector3 _direction = Vector3.forward;

    public virtual void OnInitialized()
    {
        _currentCoolTime = 0;
    }

    public virtual void StartCooltime()
    {
        if (!IsReady) return;

        _currentCoolTime = coolTime;
    }

    public virtual void AddGlobalCooltime(float globalCooltime)
    {
        if (!hasGlobalCoolTime) return;
        if (_currentCoolTime > globalCooltime) return;
        _currentCoolTime = globalCooltime;
    }

    public virtual void Tick(float deltaTime)
    {
        if (!IsReady)
        {
            _currentCoolTime -= deltaTime;
            if (_currentCoolTime <= 0)
            {
                _currentCoolTime = 0;
            }
        }
    }

    public virtual void SetDirection(Vector3 direction)
    {
        this._direction = direction.normalized;
    }

    protected abstract int GetDamage(IAttackable attacker);

    public abstract void ExecuteAttack();

    public abstract void OnEnterAttack(IAttackable attacker, Transform target = null);
    public abstract void OnExitAttack();
}
