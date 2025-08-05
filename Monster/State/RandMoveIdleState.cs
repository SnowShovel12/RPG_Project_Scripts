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
        if (context.Target != null)
        {
            stateMachine.ChangeState<MoveState>();
        }
        else
        {
            stateMachine.ChangeState<RandomMoveState>();
        }
    }
}
