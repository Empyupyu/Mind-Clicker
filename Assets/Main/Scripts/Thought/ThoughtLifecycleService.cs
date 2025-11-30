using System;
using System.Collections.Generic;
using System.Linq;

public class ThoughtLifecycleService : IThoughtLifecycleService, IDisposable
{
    public event Action<NegativeThought> OnDestroy;

    private readonly IThoughtViewPool viewPool;
    private readonly List<NegativeThought> activeThoughts = new();
    private readonly Dictionary<NegativeThought, ThoughtUIView> viewMap = new();

    public NegativeThought GetTarget() => activeThoughts.FirstOrDefault();
    public ThoughtUIView GetRandomView() => activeThoughts.Count == 0 ? null : viewMap.Values.ElementAt(UnityEngine.Random.Range(0, viewMap.Count));

    public ThoughtLifecycleService(IThoughtViewPool viewPool)
    {
        this.viewPool = viewPool;
    }

    public void Register(NegativeThought thought, ThoughtUIView view)
    {
        activeThoughts.Add(thought);
        viewMap[thought] = view;

        thought.OnDeath += HandleDeath;
        thought.OnHealthChange += view.Redraw;
    }

    public void HandleDeath(NegativeThought thought)
    {
        if (!activeThoughts.Contains(thought)) return;

        Unregister(thought);

        OnDestroy?.Invoke(thought);
    }

    public void Unregister(NegativeThought thought)
    {
        thought.OnDeath -= Unregister;
        thought.OnHealthChange -= viewMap[thought].Redraw;

        viewPool.Release(thought);
        activeThoughts.Remove(thought);
        viewMap.Remove(thought);
    }

    public void UnregisterAll()
    {
        foreach (var thought in activeThoughts.ToList())
            Unregister(thought);
    }

    public void Dispose()
    {
        UnregisterAll();
    }
}
