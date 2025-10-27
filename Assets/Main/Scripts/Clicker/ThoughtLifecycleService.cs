using System;
using System.Collections.Generic;
using System.Linq;

public class ThoughtLifecycleService : IThoughtLifecycleService
{
    private readonly IThoughtViewPool viewPool;
    private readonly List<NegativeThought> activeThoughts = new();
    private readonly Dictionary<NegativeThought, ThoughtUIView> viewMap = new();

    public event Action<NegativeThought> OnDestroy;

    public ThoughtLifecycleService(IThoughtViewPool viewPool)
    {
        this.viewPool = viewPool;
    }

    public NegativeThought GetTarget() => activeThoughts.FirstOrDefault();
    public ThoughtUIView GetRandomView() => activeThoughts.Count == 0 ? null : viewMap.Values.ElementAt(UnityEngine.Random.Range(0, viewMap.Count));

    public void Register(NegativeThought thought, ThoughtUIView view)
    {
        activeThoughts.Add(thought);
        viewMap[thought] = view;

        thought.OnDeath += HandleDeath;
        thought.OnHealthChange += view.Redraw;
    }

    private void HandleDeath(NegativeThought thought)
    {
        if (!activeThoughts.Contains(thought)) return;

        thought.OnDeath -= HandleDeath;
        thought.OnHealthChange -= viewMap[thought].Redraw;

        viewPool.Release(thought);
        activeThoughts.Remove(thought);
        viewMap.Remove(thought);

        OnDestroy?.Invoke(thought);
    }

    public void Unregister(NegativeThought thought)
    {
        HandleDeath(thought);
    }

    public void UnregisterAll()
    {
        foreach (var thought in activeThoughts.ToList())
            HandleDeath(thought);
    }
}
