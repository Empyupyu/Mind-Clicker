using Cysharp.Threading.Tasks;

public class InitializeState : IGameState
{
    private PlayerData playerData;
    private readonly SaveLoadService saveLoadService;
    private readonly GameFlowController gameFlowController;
    private const string PLAYER_DATA = "PlayerSaveData";

    public InitializeState(PlayerData playerData, SaveLoadService saveLoadService, GameFlowController gameFlowController)
    {
        this.playerData = playerData;
        this.saveLoadService = saveLoadService;
        this.gameFlowController = gameFlowController;
    }

    public async UniTask Enter()
    {
        playerData = await saveLoadService.Load(PLAYER_DATA);

        gameFlowController.LoadingLevel();
    }

    public UniTask Exit()
    {
        return UniTask.CompletedTask;
    }
}
