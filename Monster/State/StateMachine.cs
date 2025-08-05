using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class StateMachine<T>
{
    private T context;

    private State<T> currentState;
    public State<T> CurrentState => currentState;

    private State<T> priviousState;
    public State<T> PriviousState => priviousState;

    private Dictionary<System.Type, State<T>> states = new Dictionary<System.Type, State<T>>();

    private float elapsedTime = 0f;
    public float ElapsedTime => elapsedTime;

    public StateMachine(T context, State<T> initialState)
    {
        this.context = context;

        AddState(initialState);
        currentState = initialState;
    }

    public void AddState(State<T> state)
    {
        state.SetStateMachine(this, context);
        states[state.GetType()] = state;
    }

    public void Update(float deltaTime)
    {
        elapsedTime += deltaTime;

        currentState.Update(deltaTime);
    }

    public R ChangeState<R>() where R : State<T>
    {
        var newType = typeof(R);
        if (newType == currentState.GetType())
        {
            return currentState as R;
        }

        if (currentState != null)
        {
            currentState.OnExitState();
        }

        priviousState = currentState;
        currentState = states[newType];
        currentState.OnEnterState();
        elapsedTime = 0f;

        return currentState as R;
    }
}
