using Cysharp.Threading.Tasks;

public class BossThoughtLogic : IThoughtLogic
{
    public ThoughtType ThoughtType { get; }

    private readonly Mind mind;
    private readonly Timer timer;
    private readonly ThoughtSpawner thoughtSpawner;

    public BossThoughtLogic(ThoughtType thoughtType, Mind mind, Timer timer, ThoughtSpawner thoughtSpawner) 
    {
        ThoughtType = thoughtType;
        this.mind = mind;
        this.timer = timer;
        this.thoughtSpawner = thoughtSpawner;
    }

    public void AttachTo(NegativeThought thought)
    {
        thoughtSpawner.DestroyAllThought();

        timer.Initialize();
        timer.StartTimer(60).Forget();

        timer.OnFinished += () =>
        {
            mind.ReduceLevel();
            thoughtSpawner.DestroyAllThought();
            thoughtSpawner.SpawnThought();
            timer.Disable();
        };

        thought.OnDeath += (x) =>
        {
            timer.Disable();
            mind.LevelUp();
        };
    }
}
