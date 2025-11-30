using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Zenject;

public class ThoughtSpawner : IInitializable, IThoughtSpawner, IDisposable
{
    public event Action OnSpawn;
    public event Action<NegativeThought> OnDestroy;

    private ThoughtFactory factory;
    private UniTaskCompletionSource spawnDelaySource;
    private Action<NegativeThought> onThoughtDestroyedHandler;
    private Dictionary<ThoughtType, IThoughtHandler> thoughtsDic;

    private readonly IThoughtFormSelector formSelector;
    private readonly ISpawnPointSelector spawnPointSelector;
    private readonly IThoughtViewPool viewPool;
    private readonly IThoughtLifecycleService lifecycle;
    private readonly ISpawnTimingCalculator timing;
    private readonly SphereArcSpawner sphereArcSpawner;
    private readonly NegativeThoughtConfig config;
    private readonly PlayerDataRef playerData;
    private readonly ThoughtFactory thoughtFactory;
    private readonly List<IThoughtHandler> thoughtLogics;

    public NegativeThought GetTarget() => lifecycle.GetTarget();
    public ThoughtUIView GetRandomView() => lifecycle.GetRandomView();

    public ThoughtSpawner(
        IThoughtFormSelector formSelector,
        ISpawnPointSelector spawnPointSelector,
        SphereArcSpawner sphereArcSpawner,
        IThoughtViewPool viewPool,
        IThoughtLifecycleService lifecycle,
        ISpawnTimingCalculator timing,
        NegativeThoughtConfig config,
        PlayerDataRef playerData,
        ThoughtFactory thoughtFactory,
        List<IThoughtHandler> thoughtLogics)
    {
        this.formSelector = formSelector;
        this.spawnPointSelector = spawnPointSelector;
        this.sphereArcSpawner = sphereArcSpawner;
        this.viewPool = viewPool;
        this.lifecycle = lifecycle;
        this.timing = timing;
        this.config = config;
        this.playerData = playerData;
        this.thoughtFactory = thoughtFactory;
        this.thoughtLogics = thoughtLogics;
    }

    public void Initialize()
    {
        onThoughtDestroyedHandler = thought => OnDestroy?.Invoke(thought);
        lifecycle.OnDestroy += onThoughtDestroyedHandler;

        thoughtsDic = new Dictionary<ThoughtType, IThoughtHandler>();

        for (int i = 0; i < thoughtLogics.Count; i++)
        {
            IThoughtHandler logic = thoughtLogics[i];
            thoughtsDic.Add(logic.ThoughtType, logic);
        }
    }

    public void SetFactory(ThoughtFactory thoughtFactory)
    {
        factory = thoughtFactory;
    }

    public void Spawn()
    {
        spawnDelaySource?.TrySetCanceled();

        var form = formSelector.SelectRandom(playerData.Value.MindLevel);
        var thought = factory.GetThought(form, playerData.Value.MindLevel);

        bool thoughtHandlerFound = thoughtsDic.TryGetValue(form.ThoughtType, out IThoughtHandler thoughtHandler);

        if (thoughtHandlerFound == false) 
            throw new NotImplementedException(form.ThoughtType.ToString());

        thoughtHandler.AttachTo(thought);

        var view = viewPool.Get();

        view.Redraw(thought);
        
        var spawnPoint = spawnPointSelector.Select(form.SpawnPointDirection);
        sphereArcSpawner.OnSpawnCompleted += OnSpawnComplete;

        view.Initialize(thought, spawnPoint);
        viewPool.Register(view);
        lifecycle.Register(thought, view);
        view.Icon.sprite = form.Icon;

        sphereArcSpawner.Spawn(view, form, spawnPoint);

        SpawnWithDelay().Forget();
    }

    public async UniTask SpawnWithDelay()
    {
        spawnDelaySource = new UniTaskCompletionSource();

        float interval = timing.CalculateInterval(playerData.Value.MindLevel);
        var delayTask = UniTask.Delay((int)(interval * 1000));
        var controlTask = spawnDelaySource.Task;

        await UniTask.WhenAny(delayTask, controlTask);
        await UniTask.WaitUntil(() => lifecycle.GetTarget() == null || config.MaxThoughtsInGame > viewPool.GetPoolCount());

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

    private void OnSpawnComplete(SpawnPoint spawnPoint)
    {
        sphereArcSpawner.OnSpawnCompleted -= OnSpawnComplete;
        spawnPoint.ThoughtUIView.Thought.IsActive = true;
        OnSpawn?.Invoke();
    }
}