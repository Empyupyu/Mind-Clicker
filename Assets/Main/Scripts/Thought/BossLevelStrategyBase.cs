using System;
using Zenject;

public abstract class BossLevelStrategyBase : ILevelStrategy, IDisposable
{
    public ThoughtType ThoughtType { get; }

    private readonly IThoughtLifecycleService thoughtLifecycleService;
    private readonly MindProgress mindProgress;
    private readonly SignalBus signalBus;

    protected readonly ThoughtSpawner thoughtSpawner;
    protected readonly BossFightPrepare bossFightPrepare;
    protected readonly BossEnvironmentController bossEnvironmentController;
    protected NegativeThoughtForm form;

    protected BossLevelStrategyBase(
        ThoughtType thoughtType,
        BossFightPrepare bossFightPrepare,
        IThoughtLifecycleService thoughtLifecycleService,
        ThoughtSpawner thoughtSpawner,
        MindProgress mindProgress,
        SignalBus signalBus,
        BossEnvironmentController bossEnvironmentController)
    {
        ThoughtType = thoughtType;
        this.bossFightPrepare = bossFightPrepare;
        this.thoughtLifecycleService = thoughtLifecycleService;
        this.thoughtSpawner = thoughtSpawner;
        this.mindProgress = mindProgress;
        this.signalBus = signalBus;
        this.bossEnvironmentController = bossEnvironmentController;
    }

    protected virtual void CleanUp() 
    {
        bossEnvironmentController.Cleanup();
    }

    public virtual void Run(NegativeThoughtForm form)
    {
        this.form = form;
        thoughtSpawner.DestroyAll();
        bossFightPrepare.Prepare();

        signalBus.Subscribe<PrestigeSignal>(CleanUp);

        bossFightPrepare.OnTimerFinished += OnTimerFinished;
        thoughtLifecycleService.OnDestroy += OnBossDeath;
    }

    protected virtual void OnBossDeath(NegativeThought negativeThought)
    {
        bossFightPrepare.OnBossDeath(negativeThought);
    }

    protected virtual void OnTimerFinished()
    {
        mindProgress.ReduceLevel();
        thoughtSpawner.DestroyAll();
    }

    public virtual void Dispose()
    {
        signalBus.TryUnsubscribe<PrestigeSignal>(CleanUp);

        bossFightPrepare.OnTimerFinished -= OnTimerFinished;
        thoughtLifecycleService.OnDestroy -= OnBossDeath;
    }

    public void Exit()
    {
        signalBus.TryUnsubscribe<PrestigeSignal>(CleanUp);

        bossFightPrepare.OnTimerFinished -= OnTimerFinished;
        thoughtLifecycleService.OnDestroy -= OnBossDeath;
    }
}
