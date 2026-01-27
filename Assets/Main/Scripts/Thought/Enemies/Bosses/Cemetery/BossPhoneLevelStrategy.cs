using Cysharp.Threading.Tasks;
using Zenject;

public class BossPhoneLevelStrategy : BossLevelStrategyBase
{
    public BossPhoneLevelStrategy(
        ThoughtType thoughtType,
        BossFightPrepare bossFightPrepare,
        IThoughtLifecycleService thoughtLifecycleService,
        ThoughtSpawner thoughtSpawner,
        MindProgress mindProgress,
        BossEnvironmentController bossEnvironmentController,
        SignalBus signalBus) : 
        base(thoughtType, bossFightPrepare, thoughtLifecycleService, 
            thoughtSpawner, mindProgress, signalBus, bossEnvironmentController)
    {
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
        CleanUp();
    }

    protected override void OnBossDeath(NegativeThought negativeThought)
    {
        base.OnBossDeath(negativeThought);
        bossEnvironmentController.Cleanup();
    }
}
