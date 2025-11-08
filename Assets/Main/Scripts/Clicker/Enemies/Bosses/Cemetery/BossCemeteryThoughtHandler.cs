using Cysharp.Threading.Tasks;

public class BossCemeteryThoughtHandler : BossThoughtHandlerBase
{
    private readonly BossEnvironmentController bossEnvironmentController;

    public BossCemeteryThoughtHandler(ThoughtType thoughtType, BossFightPrepare bossFightPrepare, BossEnvironmentController bossEnvironmentController)
        : base(thoughtType, bossFightPrepare)
    {
        this.bossEnvironmentController = bossEnvironmentController;
    }

    public override void AttachTo(NegativeThought thought)
    {
        base.AttachTo(thought);

        InitializeEnvironment().Forget();
    }

    private async UniTask InitializeEnvironment()
    {
        await bossEnvironmentController.Initialize(ThoughtType);

        bossFightPrepare.StartFight();
    }

    protected override void OnTimerFinished()
    {
        bossEnvironmentController.Cleanup();
    }

    protected override void OnBossDeath(NegativeThought negativeThought)
    {
        base.OnBossDeath(negativeThought);
        bossEnvironmentController.Cleanup();
    }
}
