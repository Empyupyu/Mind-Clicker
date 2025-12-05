using System;
using Zenject;

public class ThoughtSpawner : IInitializable, IThoughtSpawner, IDisposable
{
    public event Action OnSpawn;
    public event Action<NegativeThought> OnDestroy;

    private Action<NegativeThought> onThoughtDestroyedHandler;

    private readonly ISpawnPointSelector spawnPointSelector;
    private readonly IThoughtViewPool viewPool;
    private readonly IThoughtLifecycleService lifecycle;
    private readonly SphereArcSpawner sphereArcSpawner;
    private readonly PlayerDataRef playerData;
    private readonly ThoughtFactory thoughtFactory;

    public NegativeThought GetTarget() => lifecycle.GetTarget();
    public ThoughtUIView GetRandomView() => lifecycle.GetRandomView();

    public ThoughtSpawner(
        ISpawnPointSelector spawnPointSelector,
        SphereArcSpawner sphereArcSpawner,
        IThoughtViewPool viewPool,
        IThoughtLifecycleService lifecycle,
        PlayerDataRef playerData,
        ThoughtFactory thoughtFactory)
    {
        this.spawnPointSelector = spawnPointSelector;
        this.sphereArcSpawner = sphereArcSpawner;
        this.viewPool = viewPool;
        this.lifecycle = lifecycle;
        this.playerData = playerData;
        this.thoughtFactory = thoughtFactory;
    }

    public void Initialize()
    {
        onThoughtDestroyedHandler = thought => OnDestroy?.Invoke(thought);
        lifecycle.OnDestroy += onThoughtDestroyedHandler;
    }

    public void Spawn(NegativeThoughtForm form)
    {
        var thought = thoughtFactory.GetThought(form, playerData.Value.MindLevel);

        var view = viewPool.Get();
        view.Redraw(thought);
        
        var spawnPoint = spawnPointSelector.Select(form.SpawnPointDirection);
        sphereArcSpawner.OnSpawnCompleted += OnSpawnComplete;

        view.Initialize(thought, spawnPoint);
        viewPool.Register(view);
        lifecycle.Register(thought, view);
        view.Icon.sprite = form.Icon;

        sphereArcSpawner.Spawn(view, form, spawnPoint);
    }

    public void DestroyAll()
    {
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

    private void OnSpawnComplete(SpawnPoint spawnPoint)
    {
        sphereArcSpawner.OnSpawnCompleted -= OnSpawnComplete;
        spawnPoint.ThoughtUIView.Thought.IsActive = true;
        OnSpawn?.Invoke();
    }
}