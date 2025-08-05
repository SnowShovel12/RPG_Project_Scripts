using UnityEngine;

public abstract class State<T>
{
    protected T context;
    protected StateMachine<T> stateMachine;

    public void SetStateMachine(StateMachine<T> stateMachine, T context)
    {
        this.context = context;
        this.stateMachine = stateMachine;

        OnInitialized();
    }

    public abstract void OnInitialized();

    public abstract void OnEnterState();

    public abstract void Update(float deltaTime);

    public abstract void OnExitState();
}
