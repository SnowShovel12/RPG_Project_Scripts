using UnityEngine;

public class HostileMonster : Monster
{
    private FieldOfView fov;

    protected override void Awake()
    {
        base.Awake();
        stateMachine.AddState(new RandMoveIdleState());
        fov = GetComponent<FieldOfView>();
    }
    protected override void Start()
    {
        base.Start();
        MakeIdleState();
    }

    protected override void Update()
    {
        base.Update();
        if (target == null)
        {
            target = fov.NearestTarget;
            if (target != null)
            {
                stateMachine.ChangeState<MoveState>();
            }
        }
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
