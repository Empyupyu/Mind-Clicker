using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class GameStateMachine
{
    private readonly Dictionary<Type, IGameState> states = new();
    private IGameState currentState;

    public void Register<T>(IGameState state) where T : IGameState
    {
        states[typeof(T)] = state;
    }
    
    public async UniTask ChangeState<T>() where T : IGameState
    { 
        if(currentState != null) 
            await currentState.Exit();
        
        currentState = states[typeof(T)];
        await currentState.Enter();
    }
}