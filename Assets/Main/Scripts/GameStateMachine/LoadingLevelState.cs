using Cysharp.Threading.Tasks;
using Main.Scripts.Views;
using UnityEngine;
using Zenject;

//TODO
public class LoadingLevelState : IGameState
{
    private readonly IGameStateView _loadingView;
    private readonly GameFlowController flowController;
    private readonly GameData gameData;
    private readonly AddressableAssetLoader addressableAssetLoader;
    private readonly DiContainer diContainer;

    public LoadingLevelState(IGameStateView loadingView, GameFlowController flowController, GameData gameData, AddressableAssetLoader addressableAssetLoader, DiContainer diContainer)
    {
        this._loadingView = loadingView;
        this.flowController = flowController;
        this.gameData = gameData;
        this.addressableAssetLoader = addressableAssetLoader;
        this.diContainer = diContainer;
    }

    public async UniTask Enter()
    {
        _loadingView.Show();

        var handle = await addressableAssetLoader.LoadAsset<GameObject>("Game");

        await UniTask.Delay(500);

        //var sceneContext = GameObject.FindObjectOfType<SceneContext>();
        //var level = sceneContext.Container.InstantiatePrefab(handle);
        var level = diContainer.InstantiatePrefab(handle);

        var gameContext = level.GetComponent<GameObjectContext>();
        gameData.Level = gameContext;
        //gameContext.Install(sceneContext.Container);
        gameContext.Install(diContainer);
        gameContext.Run();

        await UniTask.Delay(1500);

        flowController.LevelGamePlay();
    }

    public async UniTask Exit()
    {
        await _loadingView.Hide();
    }
}
