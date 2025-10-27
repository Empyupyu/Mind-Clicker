using Cysharp.Threading.Tasks;

public class BossCemeteryThoughtLogic : BossThoughtLogicBase
{
    private readonly CemeteryEnvironmentController cemeteryEnvironmentController;

    public BossCemeteryThoughtLogic(ThoughtType thoughtType, BossFightPrepare bossFightPrepare, CemeteryEnvironmentController cemeteryEnvironmentController)
        : base(thoughtType, bossFightPrepare)
    {
        this.cemeteryEnvironmentController = cemeteryEnvironmentController;
    }

    public override void AttachTo(NegativeThought thought)
    {
        base.AttachTo(thought);

        InitializeEnvironment().Forget();
    }

    private async UniTask InitializeEnvironment()
    {
        await cemeteryEnvironmentController.Initialize();

        bossFightPrepare.StartFight();
    }

    protected override void OnTimerFinished()
    {
        cemeteryEnvironmentController.Cleanup();
    }

    protected override void OnBossDeath(NegativeThought negativeThought)
    {
        base.OnBossDeath(negativeThought);
        cemeteryEnvironmentController.Cleanup();
    }
}
