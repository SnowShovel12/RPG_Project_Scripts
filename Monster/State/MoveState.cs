using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;

public class MoveState : State<Monster>
{
    private CharacterController controller;
    private NavMeshAgent agent;
    private Animator animator;

    private int animatorVelocityHash = Animator.StringToHash("Velocity");

    private Vector3 currentVelocity;

    private Vector3 TargetPosition => context.Target.position;

    public override void OnInitialized()
    {
        controller = context.controller;
        agent = context.agent;
        animator = context.animator;
    }

    public override void OnEnterState()
    {
        agent.SetDestination(TargetPosition);
    }

    public override void OnExitState()
    {
        agent.ResetPath();
    }

    public override void Update(float deltaTime)
    {
        if (context.CurrentAttack != null)
        {
            agent.stoppingDistance = context.AttackRange * 0.8f;
        }
        agent.SetDestination(TargetPosition);

        if (agent.remainingDistance <= agent.stoppingDistance + 0.05f)
        {
            currentVelocity = Vector3.zero;
        }
        else
        {
            Vector3 desiredVelocity = new Vector3(agent.desiredVelocity.x, 0, agent.desiredVelocity.z);
            currentVelocity = Vector3.Lerp(currentVelocity, desiredVelocity, agent.acceleration * deltaTime);
        }

        if (currentVelocity != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(currentVelocity.normalized);
            context.transform.rotation = Quaternion.Slerp(context.transform.rotation, lookRotation, deltaTime * 10f);
        }
        controller.Move(currentVelocity * deltaTime);
        agent.nextPosition = context.transform.position;
        animator.SetFloat(animatorVelocityHash, currentVelocity.magnitude / context.MoveSpeed);

        if (agent.remainingDistance <= agent.stoppingDistance + 0.1f && context.CurrentAttack != null && context.CurrentAttack.IsReady && context.IsAlive)
        {
            stateMachine.ChangeState<AttackState>();
        }
    }
}
