using Cysharp.Threading.Tasks;

public class InitializeState : IGameState
{
    private PlayerDataRef playerData;
    private readonly SaveLoadService saveLoadService;
    private readonly GameFlowController gameFlowController;
    private const string PLAYER_DATA = "PlayerSaveData";

    public InitializeState(PlayerDataRef playerData, SaveLoadService saveLoadService, GameFlowController gameFlowController)
    {
        this.playerData = playerData;
        this.saveLoadService = saveLoadService;
        this.gameFlowController = gameFlowController;
    }

    public async UniTask Enter()
    {
        var data = await saveLoadService.Load(PLAYER_DATA);
        playerData.Set(data);

        gameFlowController.LoadingLevel();
    }

    public UniTask Exit()
    {
        return UniTask.CompletedTask;
    }
}
