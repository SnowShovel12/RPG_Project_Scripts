using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class StateMachine<T>
{
    private T _context;

    private State<T> _currentState;
    public State<T> CurrentState => _currentState;

    private State<T> _priviousState;
    public State<T> PriviousState => _priviousState;

    private Dictionary<System.Type, State<T>> _states = new Dictionary<System.Type, State<T>>();

    private float _elapsedTime = 0f;
    public float ElapsedTime => _elapsedTime;

    public StateMachine(T context, State<T> initialState)
    {
        _context = context;

        AddState(initialState);
        _currentState = initialState;
    }

    public void AddState(State<T> state)
    {
        state.SetStateMachine(this, _context);
        _states[state.GetType()] = state;
    }

    public void Update(float deltaTime)
    {
        _elapsedTime += deltaTime;

        _currentState.Update(deltaTime);
    }

    public R ChangeState<R>() where R : State<T>
    {
        var newType = typeof(R);
        if (newType == _currentState.GetType())
        {
            return _currentState as R;
        }

        if (_currentState != null)
        {
            _currentState.OnExitState();
        }

        _priviousState = _currentState;
        _currentState = _states[newType];
        _currentState.OnEnterState();
        _elapsedTime = 0f;

        return _currentState as R;
    }
}
