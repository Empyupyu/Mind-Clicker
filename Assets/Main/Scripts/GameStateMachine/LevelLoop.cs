using Cysharp.Threading.Tasks;
using Zenject;

public class LevelLoop : ILevelLoop, IInitializable
{
    private readonly LevelMusicFlow levelMusicFlow;
    private readonly SignalBus signalBus;
    private ThoughtSpawner thoughtSpawner;
    private readonly MindLevelPresentation mindLevelPresentation;

    public LevelLoop(
        LevelMusicFlow levelMusicFlow,
        SignalBus signalBus,
        ThoughtSpawner thoughtSpawner,
        MindLevelPresentation mindLevelPresentation)
    {
        this.levelMusicFlow = levelMusicFlow;
        this.signalBus = signalBus;
        this.thoughtSpawner = thoughtSpawner;
        this.mindLevelPresentation = mindLevelPresentation;
    }

    public void Initialize()
    {
        signalBus.Subscribe<GameLoadedSignal>(OnGameLoaded);

        mindLevelPresentation.OnLevelUpAnimationEnded += LoadLevelStrategy;
    }

    private void LoadLevelStrategy()
    {
        throw new System.NotImplementedException();
    }

    private void OnGameLoaded(GameLoadedSignal gameLoadedSignal)
    {
        StartGameAsync().Forget();
    }

    public async UniTask StartGameAsync()
    {
        await levelMusicFlow.Opening();
        await UniTask.Delay(1000);

        thoughtSpawner.Spawn();
    }
}
