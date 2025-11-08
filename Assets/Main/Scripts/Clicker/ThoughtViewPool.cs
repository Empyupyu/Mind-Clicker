using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThoughtViewPool : IThoughtViewPool
{
    private readonly ThoughtUIView prefab;
    private readonly SphereArcSpawner sphereArcSpawner;
    private readonly Dictionary<NegativeThought, ThoughtUIView> map = new();

    public ThoughtViewPool(ThoughtUIView prefab, SphereArcSpawner sphereArcSpawner)
    {
        this.prefab = prefab;
        this.sphereArcSpawner = sphereArcSpawner;
    }

    public ThoughtUIView Get()
    {
        var view = GameObject.Instantiate(prefab);
        view.gameObject.SetActive(false);
        return view;
    }

    public void Register(ThoughtUIView view)
    {
        map[view.Thought] = view;
    }

    public void Release(NegativeThought thought)
    {
        if (!map.TryGetValue(thought, out var view)) return;

        if (view != null && view.gameObject != null)
            sphereArcSpawner.Disable(view.SpawnPoint);

        map.Remove(thought);
    }

    public ThoughtUIView GetRandom()
    {
        if (map.Count == 0) return null;
        return map.Values.ElementAt(Random.Range(0, map.Count));
    }

    public int GetPoolCount()
    {
        return map.Count;
    }
}
