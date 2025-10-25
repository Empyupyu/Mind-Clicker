public class BossBubbleThoughtLogic : BossThoughtLogicBase
{
    public BossBubbleThoughtLogic(ThoughtType thoughtType, BossFightPrepare bossFightPrepare)
        : base(thoughtType, bossFightPrepare) { }


    public override void AttachTo(NegativeThought thought)
    {
        base.AttachTo(thought);

        bossFightPrepare.StartFight();
    }

    protected override void OnTimerFinished() { }
}
