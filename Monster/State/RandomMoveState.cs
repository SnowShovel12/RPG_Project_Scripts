using UnityEngine;
using UnityEngine.AI;

public class RandomMoveState : State<Monster>
{
    private CharacterController controller;
    private NavMeshAgent agent;
    private Animator animator;
    private Vector3 nextPosition;

    private int animatorVelocityHash = Animator.StringToHash("Velocity");

    public override void OnInitialized()
    {
        controller = context.controller;
        agent = context.agent;
        animator = context.animator;
    }

    public override void OnEnterState()
    {
        nextPosition = SetRandomPosition(15f, 15f);
        agent.SetDestination(nextPosition);
        agent.stoppingDistance = 0;
    }

    public override void OnExitState()
    {
        agent.ResetPath();
    }

    public override void Update(float deltaTime)
    {
        Vector3 direction = new Vector3(agent.desiredVelocity.x, 0 , agent.desiredVelocity.z);
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction.normalized);
            context.transform.rotation = Quaternion.Slerp(context.transform.rotation, lookRotation, deltaTime * 10f);
        }
        controller.Move(direction * deltaTime);
        agent.nextPosition = context.transform.position;
        animator.SetFloat(animatorVelocityHash, direction.magnitude / context.MoveSpeed);

        if (agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            nextPosition = SetRandomPosition(15f, 15f);
            agent.SetDestination(nextPosition);
        }
    }

    private Vector3 SetRandomPosition(float x, float y)
    {
        float randomX = Random.Range(-x / 2, x / 2);
        float randomY = Random.Range(-y / 2, y / 2);

        Vector3 originPosition = context.transform.position;
        Vector3 randomPosition = originPosition + new Vector3(randomX, 0, randomY);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, 2.0f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            return SetRandomPosition(x, y);
        }
    }
}
