using UnityEngine;

public class RandMoveIdleState : State<Monster>
{
    public override void OnEnterState()
    {

    }

    public override void OnExitState()
    {

    }

    public override void OnInitialized()
    {

    }

    public override void Update(float deltaTime)
    {
        if (_context.Target != null)
        {
            _stateMachine.ChangeState<MoveState>();
        }
        else
        {
            _stateMachine.ChangeState<RandomMoveState>();
        }
    }
}
