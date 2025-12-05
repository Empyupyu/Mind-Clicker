public class BossBubbleLevelStrategy : BossLevelStrategyBase
{
    public BossBubbleLevelStrategy(
        ThoughtType thoughtType,
        BossFightPrepare bossFightPrepare,
        IThoughtLifecycleService thoughtLifecycleService,
        ThoughtSpawner thoughtSpawner,
        MindProgress mindProgress,
        BossEnvironmentController bossEnvironmentController) :
        base(thoughtType, bossFightPrepare, thoughtLifecycleService, thoughtSpawner, mindProgress)
    {

    }

    public override void Run(NegativeThoughtForm form)
    {
        base.Run(form);

        bossFightPrepare.StartFight();
    }

    protected override void OnTimerFinished() { }
}
