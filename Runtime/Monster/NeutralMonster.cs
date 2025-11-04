using UnityEngine;

public class NeutralMonster : Monster
{
    protected override void Awake()
    {
        base.Awake();
        stateMachine.AddState(new RandMoveIdleState());
    }

    protected override void Start()
    {
        base.Start();
        MakeIdleState();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public override void MakeIdleState()
    {
        stateMachine.ChangeState<RandMoveIdleState>();
    }

    public override void TakeDamage(int damage, IAttackable attacker, StatusEffectSO statusEffect = null, float duration = 0)
    {
        base.TakeDamage(damage, attacker, statusEffect, duration);
        if (IsAlive)
        {
            target = attacker.Transform;
            stateMachine.ChangeState<MoveState>();
        }
    }

}
