using UnityEngine;

public abstract class State<T>
{
    protected T _context;
    protected StateMachine<T> _stateMachine;

    public void SetStateMachine(StateMachine<T> stateMachine, T context)
    {
        _context = context;
        _stateMachine = stateMachine;

        OnInitialized();
    }

    public abstract void OnInitialized();

    public abstract void OnEnterState();

    public abstract void Update(float deltaTime);

    public abstract void OnExitState();
}
