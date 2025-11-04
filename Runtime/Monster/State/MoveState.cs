using UnityEngine;
using UnityEngine.AI;

public class MoveState : State<Monster>
{
    private CharacterController _controller;
    private NavMeshAgent _agent;
    private Animator _animator;

    private readonly int _animatorVelocityHash = Animator.StringToHash("Velocity");

    private Vector3 _currentVelocity;

    private Vector3 _TargetPosition => _context.Target.position;

    public override void OnInitialized()
    {
        _controller = _context.controller;
        _agent = _context.agent;
        _animator = _context.animator;
    }

    public override void OnEnterState()
    {
        _agent.SetDestination(_TargetPosition);
    }

    public override void OnExitState()
    {
        _agent.ResetPath();
    }

    public override void Update(float deltaTime)
    {
        if (_context.CurrentAttack != null)
        {
            _agent.stoppingDistance = _context.AttackRange * 0.8f;
        }
        _agent.SetDestination(_TargetPosition);

        if (_agent.remainingDistance <= _agent.stoppingDistance + 0.05f)
        {
            _currentVelocity = Vector3.zero;
        }
        else
        {
            Vector3 desiredVelocity = new Vector3(_agent.desiredVelocity.x, 0, _agent.desiredVelocity.z);
            _currentVelocity = Vector3.Lerp(_currentVelocity, desiredVelocity, _agent.acceleration * deltaTime);
        }

        if (_currentVelocity != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(_currentVelocity.normalized);
            _context.transform.rotation = Quaternion.Slerp(_context.transform.rotation, lookRotation, deltaTime * 10f);
        }
        _controller.Move(_currentVelocity * deltaTime);
        _agent.nextPosition = _context.transform.position;
        _animator.SetFloat(_animatorVelocityHash, _currentVelocity.magnitude / _context.MoveSpeed);

        if (_agent.remainingDistance <= _agent.stoppingDistance + 0.1f && _context.CurrentAttack != null && _context.CurrentAttack.IsReady && _context.IsAlive)
        {
            _stateMachine.ChangeState<AttackState>();
        }
    }
}
