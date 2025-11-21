using Cysharp.Threading.Tasks;
using Zenject;

public class GameLoop : IGameLoop, IInitializable
{
    private readonly LevelMusicFlow levelMusicFlow;
    private readonly SignalBus signalBus;
    private readonly DiContainer di;
    private ThoughtSpawner thoughtSpawner;

    public GameLoop(LevelMusicFlow levelMusicFlow, SignalBus signalBus, ThoughtSpawner thoughtSpawner)
    {
        this.levelMusicFlow = levelMusicFlow;
        this.signalBus = signalBus;
        this.thoughtSpawner = thoughtSpawner;
    }

    public void Initialize()
    {
        signalBus.Subscribe<LevelLoadedSignal>(OnLevelLoaded);
    }

    private void OnLevelLoaded(LevelLoadedSignal levelLoadedSignal)
    {
        StartAsync().Forget();
    }

    public async UniTask StartAsync()
    {
        await levelMusicFlow.Opening();
        await UniTask.Delay(1000);

        thoughtSpawner.Spawn();
    }
}
