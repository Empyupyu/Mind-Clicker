using Cysharp.Threading.Tasks;

public class Tier1EnemyLevelStrategy : ILevelStrategy
{
    public ThoughtType ThoughtType { get; }

    private readonly ThoughtSpawner thoughtSpawner;
    private readonly PlayerDataRef playerData;
    private readonly ISpawnTimingCalculator timing;
    private readonly IThoughtLifecycleService lifecycle;
    private readonly NegativeThoughtConfig config;
    private readonly IThoughtViewPool viewPool;
    private UniTaskCompletionSource spawnDelaySource;

    public Tier1EnemyLevelStrategy(
        ThoughtType thoughtType,
        ThoughtSpawner thoughtSpawner,
        PlayerDataRef playerData,
        ISpawnTimingCalculator timing,
        IThoughtLifecycleService lifecycle,
        NegativeThoughtConfig config,
        IThoughtViewPool viewPool) 
    {
        ThoughtType = thoughtType;
        this.thoughtSpawner = thoughtSpawner;
        this.playerData = playerData;
        this.timing = timing;
        this.lifecycle = lifecycle;
        this.config = config;
        this.viewPool = viewPool;
    }

    public void Run(NegativeThoughtForm form) 
    {
        thoughtSpawner.Spawn(form);
        SpawnWithDelay(form).Forget();
    }

    public async UniTask SpawnWithDelay(NegativeThoughtForm form)
    {
        spawnDelaySource = new UniTaskCompletionSource();

        float interval = timing.CalculateInterval(playerData.Value.MindLevel);
        var delayTask = UniTask.Delay((int)(interval * 1000));
        var controlTask = spawnDelaySource.Task;

        await UniTask.WhenAny(delayTask, controlTask);

        await UniTask.WaitUntil(() => lifecycle.GetTarget() == null || 
        viewPool.GetPoolCount() < config.MaxThoughtsInGame);

        if (controlTask.Status == UniTaskStatus.Canceled)
            return;

        thoughtSpawner.Spawn(form);
    }

    public void Exit()
    {
        spawnDelaySource?.TrySetCanceled();
    }
}
