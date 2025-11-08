using System;

public abstract class BossThoughtHandlerBase : IThoughtHandler, IDisposable
{
    public ThoughtType ThoughtType { get; }

    protected readonly BossFightPrepare bossFightPrepare;
    protected NegativeThought currentNegativeThought;

    protected BossThoughtHandlerBase(ThoughtType thoughtType, BossFightPrepare bossFightPrepare)
    {
        ThoughtType = thoughtType;
        this.bossFightPrepare = bossFightPrepare;
    }

    public virtual void AttachTo(NegativeThought thought)
    {
        bossFightPrepare.Prepare();

        currentNegativeThought = thought;
        bossFightPrepare.OnTimerFinished += OnTimerFinished;
        currentNegativeThought.OnDeath += OnBossDeath;
    }

    protected virtual void OnBossDeath(NegativeThought negativeThought)
    {
        bossFightPrepare.OnBossDeath(negativeThought);
    }

    protected abstract void OnTimerFinished();

    public virtual void Dispose()
    {
        bossFightPrepare.OnTimerFinished -= OnTimerFinished;
        if (currentNegativeThought != null)
            currentNegativeThought.OnDeath -= OnBossDeath;
    }
}
