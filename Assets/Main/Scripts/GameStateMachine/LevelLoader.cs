using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class LevelLoader : ILevelLoader
{
    private readonly AddressableAssetLoader assetLoader;
    private readonly DiContainer diContainer;
    private readonly GameData gameData;
    private readonly SignalBus signalBus;

    public LevelLoader(
        AddressableAssetLoader assetLoader,
        DiContainer diContainer,
        GameData gameData,
        SignalBus signalBus)
    {
        this.assetLoader = assetLoader;
        this.diContainer = diContainer;
        this.gameData = gameData;
        this.signalBus = signalBus;
    }

    public async UniTask LoadLevelAsync(string key)
    {
        var prefab = await assetLoader.LoadAsset<GameObject>(key);
        var level = diContainer.InstantiatePrefab(prefab);
        var context = level.GetComponent<GameObjectContext>();

        gameData.Level = context;

        context.Install(diContainer);
        context.Run();

        await UniTask.Delay(1000);

        signalBus.Fire(new GameLoadedSignal());
    }
}
