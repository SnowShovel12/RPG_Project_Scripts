using System;
using System.Collections;
using UnityEngine;

[Serializable]
public abstract class Attack
{
    public bool hasGlobalCoolTime;

    public float coolTime;
    protected float currentCoolTime;
    public float CurrentCooltime => currentCoolTime;
    public float CooltimeRatio => currentCoolTime / coolTime;

    public bool IsReady => currentCoolTime <= 0;

    public float modifier = 1f;

    public int animationIndex;

    readonly protected int attackHash = Animator.StringToHash("Attack");
    readonly protected int attackIndexHash = Animator.StringToHash("AttackIndex");

    protected Vector3 direction = Vector3.forward;

    public virtual void OnInitialized()
    {
        currentCoolTime = 0;
    }

    public virtual void StartCooltime()
    {
        if (!IsReady) return;

        currentCoolTime = coolTime;
    }

    public virtual void AddGlobalCooltime(float globalCooltime)
    {
        if (!hasGlobalCoolTime) return;
        if (currentCoolTime > globalCooltime) return;
        currentCoolTime = globalCooltime;
    }

    public virtual void Tick(float deltaTime)
    {
        if (!IsReady)
        {
            currentCoolTime -= deltaTime;
            if (currentCoolTime <= 0)
            {
                currentCoolTime = 0;
            }
        }
    }

    public virtual void SetDirection(Vector3 direction)
    {
        this.direction = direction.normalized;
    }

    protected abstract int GetDamage(IAttackable attacker);

    public abstract void ExecuteAttack();

    public abstract void OnEnterAttack(IAttackable attacker, Transform target = null);
    public abstract void OnExitAttack();
}
