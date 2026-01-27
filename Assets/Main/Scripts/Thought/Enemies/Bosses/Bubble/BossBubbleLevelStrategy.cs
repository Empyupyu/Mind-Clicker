using Zenject;

public class BossBubbleLevelStrategy : BossLevelStrategyBase
{
    public BossBubbleLevelStrategy(
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

        bossFightPrepare.StartFight();
    }

    protected override void OnTimerFinished() { }
}
