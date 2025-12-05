using Cysharp.Threading.Tasks;

public class BossCemeteryLevelStrategy : BossLevelStrategyBase
{
    private readonly BossEnvironmentController bossEnvironmentController;

    public BossCemeteryLevelStrategy(
        ThoughtType thoughtType,
        BossFightPrepare bossFightPrepare,
        IThoughtLifecycleService thoughtLifecycleService,
        ThoughtSpawner thoughtSpawner,
        MindProgress mindProgress,
        BossEnvironmentController bossEnvironmentController) :
        base(thoughtType, bossFightPrepare, thoughtLifecycleService, thoughtSpawner, mindProgress)
    {
        this.bossEnvironmentController = bossEnvironmentController;
    }

    public override void Run(NegativeThoughtForm form)
    {
        base.Run(form);

        InitializeEnvironment().Forget();
    }

    private async UniTask InitializeEnvironment()
    {
        await bossEnvironmentController.Initialize(ThoughtType);

        bossFightPrepare.StartFight();
        thoughtSpawner.Spawn(form);
    }

    protected override void OnTimerFinished()
    {
        base.OnTimerFinished();

        bossEnvironmentController.Cleanup();
    }

    protected override void OnBossDeath(NegativeThought negativeThought)
    {
        base.OnBossDeath(negativeThought);
        bossEnvironmentController.Cleanup();
    }
}
public class BossPhoneLevelStrategy : BossLevelStrategyBase
{
    private readonly BossEnvironmentController bossEnvironmentController;

    public BossPhoneLevelStrategy(
        ThoughtType thoughtType,
        BossFightPrepare bossFightPrepare,
        IThoughtLifecycleService thoughtLifecycleService,
        ThoughtSpawner thoughtSpawner,
        MindProgress mindProgress,
        BossEnvironmentController bossEnvironmentController) : 
        base(thoughtType, bossFightPrepare, thoughtLifecycleService, thoughtSpawner, mindProgress)
    {
        this.bossEnvironmentController = bossEnvironmentController;
    }

    public override void Run(NegativeThoughtForm form)
    {
        base.Run(form);

        InitializeEnvironment().Forget();
    }

    private async UniTask InitializeEnvironment()
    {
        await bossEnvironmentController.Initialize(ThoughtType);

        bossFightPrepare.StartFight();
        thoughtSpawner.Spawn(form);
    }

    protected override void OnTimerFinished()
    {
        base.OnTimerFinished();

        bossEnvironmentController.Cleanup();
    }

    protected override void OnBossDeath(NegativeThought negativeThought)
    {
        base.OnBossDeath(negativeThought);
        bossEnvironmentController.Cleanup();
    }
}
