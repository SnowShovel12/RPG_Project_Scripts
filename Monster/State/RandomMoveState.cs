using UnityEngine;
using UnityEngine.AI;

public class RandomMoveState : State<Monster>
{
    private CharacterController _controller;
    private NavMeshAgent _agent;
    private Animator _animator;
    private Vector3 _nextPosition;

    private readonly int _animatorVelocityHash = Animator.StringToHash("Velocity");

    public override void OnInitialized()
    {
        _controller = _context.controller;
        _agent = _context.agent;
        _animator = _context.animator;
    }

    public override void OnEnterState()
    {
        _nextPosition = SetRandomPosition(15f, 15f);
        _agent.SetDestination(_nextPosition);
        _agent.stoppingDistance = 0;
    }

    public override void OnExitState()
    {
        _agent.ResetPath();
    }

    public override void Update(float deltaTime)
    {
        Vector3 direction = new Vector3(_agent.desiredVelocity.x, 0 , _agent.desiredVelocity.z);
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction.normalized);
            _context.transform.rotation = Quaternion.Slerp(_context.transform.rotation, lookRotation, deltaTime * 10f);
        }
        _controller.Move(direction * deltaTime);
        _agent.nextPosition = _context.transform.position;
        _animator.SetFloat(_animatorVelocityHash, direction.magnitude / _context.MoveSpeed);

        if (_agent.remainingDistance <= _agent.stoppingDistance + 0.1f)
        {
            _nextPosition = SetRandomPosition(15f, 15f);
            _agent.SetDestination(_nextPosition);
        }
    }

    private Vector3 SetRandomPosition(float x, float y)
    {
        float randomX = Random.Range(-x / 2, x / 2);
        float randomY = Random.Range(-y / 2, y / 2);

        Vector3 originPosition = _context.transform.position;
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
