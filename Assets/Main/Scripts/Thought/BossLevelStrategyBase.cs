using System;

public abstract class BossLevelStrategyBase : ILevelStrategy, IDisposable
{
    public ThoughtType ThoughtType { get; }

    protected readonly BossFightPrepare bossFightPrepare;
    private readonly IThoughtLifecycleService thoughtLifecycleService;
    protected readonly ThoughtSpawner thoughtSpawner;
    private readonly MindProgress mindProgress;
    protected NegativeThoughtForm form;

    protected BossLevelStrategyBase(
        ThoughtType thoughtType,
        BossFightPrepare bossFightPrepare,
        IThoughtLifecycleService thoughtLifecycleService,
        ThoughtSpawner thoughtSpawner,
        MindProgress mindProgress)
    {
        ThoughtType = thoughtType;
        this.bossFightPrepare = bossFightPrepare;
        this.thoughtLifecycleService = thoughtLifecycleService;
        this.thoughtSpawner = thoughtSpawner;
        this.mindProgress = mindProgress;
    }

    public virtual void Run(NegativeThoughtForm form)
    {
        this.form = form;
        thoughtSpawner.DestroyAll();
        bossFightPrepare.Prepare();

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
        bossFightPrepare.OnTimerFinished -= OnTimerFinished;
        thoughtLifecycleService.OnDestroy -= OnBossDeath;
    }

    public void Exit()
    {
        bossFightPrepare.OnTimerFinished -= OnTimerFinished;
        thoughtLifecycleService.OnDestroy -= OnBossDeath;
    }
}
