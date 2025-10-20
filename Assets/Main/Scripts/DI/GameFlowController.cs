using UnityEngine;
using Zenject;

public class GameFlowController : MonoBehaviour
{
    private GameStateMachine stateMachine;

    [Inject]
    private void Construct(GameStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    void Start()
    {
        StartGame();
    }

    public void StartGame() => stateMachine.ChangeState<InitializeState>();
    public void LoadingLevel() => stateMachine.ChangeState<LoadingLevelState>();
    // public void PauseGame() => stateMachine.ChangeState<PauseState>();
    public void LevelGamePlay() => stateMachine.ChangeState<LevelState>();
}