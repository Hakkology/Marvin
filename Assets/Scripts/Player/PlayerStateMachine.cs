using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState {get; private set;}
    // value public olarak her yerden ulaşılabilir ama değiştirilemez.

    public void Initialize(PlayerState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}