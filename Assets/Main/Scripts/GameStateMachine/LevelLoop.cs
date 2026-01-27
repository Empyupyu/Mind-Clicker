using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Zenject;

public class LevelLoop : ILevelLoop, IInitializable, IDisposable
{
    private readonly IThoughtFormSelector formSelector;
    private readonly LevelMusicFlow levelMusicFlow;
    private readonly SignalBus signalBus;
    private readonly MindLevelPresentation mindLevelPresentation;
    private readonly PlayerDataRef playerData;

    private Dictionary<ThoughtType, ILevelStrategy> levelStrategies;
    private ILevelStrategy currentLevelStrategy;
    private CancellationTokenSource startGameCts;

    public LevelLoop(
        List<ILevelStrategy> levelStrategies,
        IThoughtFormSelector formSelector,
        LevelMusicFlow levelMusicFlow,
        SignalBus signalBus,
        MindLevelPresentation mindLevelPresentation,
        PlayerDataRef playerData)
    {
        this.formSelector = formSelector;
        this.levelMusicFlow = levelMusicFlow;
        this.signalBus = signalBus;
        this.mindLevelPresentation = mindLevelPresentation;
        this.playerData = playerData;
        this.levelStrategies = levelStrategies.ToDictionary(s => s.ThoughtType);
    }

    public void Initialize()
    {
        signalBus.Subscribe<GameLoadedSignal>(OnGameLoaded);

        mindLevelPresentation.OnLevelUpAnimationEnded += LoadLevelStrategy;
        mindLevelPresentation.OnLevelReduceAnimationEnded += LoadLevelStrategy;
    }

    private void LoadLevelStrategy()
    {
        if (currentLevelStrategy != null) currentLevelStrategy.Exit();

        var form = formSelector.SelectRandom(playerData.Value.MindLevel);
        bool levelStrategyFound = levelStrategies.TryGetValue(form.ThoughtType, out currentLevelStrategy);

        if (levelStrategyFound == false)
            throw new NotImplementedException(form.ThoughtType.ToString());

        currentLevelStrategy.Run(form);
    }

    private void OnGameLoaded(GameLoadedSignal gameLoadedSignal)
    {
        levelMusicFlow.StopMainSoundTrack();
        startGameCts?.Cancel();
        startGameCts = new CancellationTokenSource();
        StartGameAsync(startGameCts.Token).Forget();
    }

    public async UniTask StartGameAsync(CancellationToken token)
    {
        await levelMusicFlow.Opening().AttachExternalCancellation(token);
        await UniTask.Delay(GameConstants.FakeLoadingDelay, cancellationToken: token);

        LoadLevelStrategy();
    }

    public void Dispose()
    {     
        signalBus.TryUnsubscribe<GameLoadedSignal>(OnGameLoaded);

        mindLevelPresentation.OnLevelUpAnimationEnded -= LoadLevelStrategy;
        mindLevelPresentation.OnLevelReduceAnimationEnded -= LoadLevelStrategy;

        startGameCts?.Cancel();
        startGameCts?.Dispose();
    }
}
