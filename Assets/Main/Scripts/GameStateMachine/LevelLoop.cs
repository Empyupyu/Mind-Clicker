using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

public class LevelLoop : ILevelLoop, IInitializable
{
    private readonly IThoughtFormSelector formSelector;
    private readonly LevelMusicFlow levelMusicFlow;
    private readonly SignalBus signalBus;
    private readonly MindLevelPresentation mindLevelPresentation;
    private readonly PlayerDataRef playerData;

    private Dictionary<ThoughtType, ILevelStrategy> levelStrategies;
    private ILevelStrategy currentLevelStrategy;

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
        StartGameAsync().Forget();
    }

    public async UniTask StartGameAsync()
    {
        await levelMusicFlow.Opening();
        await UniTask.Delay(GameConstants.FakeLoadingDelay);

        LoadLevelStrategy();
    }
}
