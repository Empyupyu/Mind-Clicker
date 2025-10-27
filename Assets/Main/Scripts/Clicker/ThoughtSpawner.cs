using Cysharp.Threading.Tasks;
using System;

public class ThoughtSpawner : IThoughtSpawner, IDisposable
{
    public event Action OnSpawn;
    public event Action<NegativeThought> OnDestroy;

    private readonly IThoughtFormSelector formSelector;
    private readonly ISpawnPointSelector spawnSelector;
    private readonly IThoughtViewPool viewPool;
    private readonly IThoughtLifecycleService lifecycle;
    private readonly ISpawnTimingCalculator timing;
    private readonly NegativeThoughtConfig config;
    private readonly PlayerDataRef playerData;

    private ThoughtFactory factory;
    private UniTaskCompletionSource spawnDelaySource;
    private Action<NegativeThought> onThoughtDestroyedHandler;

    public ThoughtSpawner(
        IThoughtFormSelector formSelector,
        ISpawnPointSelector spawnSelector,
        IThoughtViewPool viewPool,
        IThoughtLifecycleService lifecycle,
        ISpawnTimingCalculator timing,
        NegativeThoughtConfig config,
        PlayerDataRef playerData)
    {
        this.formSelector = formSelector;
        this.spawnSelector = spawnSelector;
        this.viewPool = viewPool;
        this.lifecycle = lifecycle;
        this.timing = timing;
        this.config = config;
        this.playerData = playerData;

        onThoughtDestroyedHandler = thought => OnDestroy?.Invoke(thought);
        lifecycle.OnDestroy += onThoughtDestroyedHandler;
    }

    public NegativeThought GetTarget() => lifecycle.GetTarget();
    public ThoughtUIView GetRandomView() => lifecycle.GetRandomView();

    public void SetFactory(ThoughtFactory thoughtFactory)
    {
        factory = thoughtFactory;
    }

    public void Spawn()
    {
        spawnDelaySource?.TrySetCanceled();

        var form = formSelector.Select(playerData.Value.MindLevel);
        var thought = factory.GetThought(form, playerData.Value.MindLevel);
        var view = viewPool.Get();

        view.Redraw(thought);
        
        var spawner = spawnSelector.Select(form.SpawnPointDirection);
        spawner.OnSpawnCompleted += OnSpawnComplete;

        view.Initialize(thought, spawner);
        viewPool.Register(view);
        lifecycle.Register(thought, view);
        view.Icon.sprite = form.Icon;

        spawner.SpawnAlongArc(view, form);
    }

    private void OnSpawnComplete(SphereArcSpawner spawner)
    {
        spawner.OnSpawnCompleted -= OnSpawnComplete;
        spawner.ThoughtUIView.Thought.IsActive = true;
        OnSpawn?.Invoke();
    }

    public async UniTask SpawnWithDelay()
    {
        spawnDelaySource = new UniTaskCompletionSource();

        float interval = timing.CalculateInterval(playerData.Value.MindLevel);
        var delayTask = UniTask.Delay((int)(interval * 1000));
        var controlTask = spawnDelaySource.Task;

        await UniTask.WhenAny(delayTask, controlTask);
        await UniTask.WaitUntil(() => lifecycle.GetTarget() == null || config.MaxThoughtsInGame > 1);

        if (controlTask.Status == UniTaskStatus.Canceled)
            return;

        Spawn();
    }

    public void DestroyAll()
    {
        spawnDelaySource?.TrySetCanceled();
        lifecycle.UnregisterAll();
    }

    public void Dispose()
    {
        if (onThoughtDestroyedHandler != null)
        {
            lifecycle.OnDestroy -= onThoughtDestroyedHandler;
            onThoughtDestroyedHandler = null;
        }

        lifecycle.UnregisterAll();
    }
}