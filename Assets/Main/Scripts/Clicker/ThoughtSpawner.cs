using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThoughtSpawner : IDisposable
{
    public event Action OnSpawn;
    public event Action OnDestroy;

    private readonly NegativeThoughtConfig thoughtConfigs;
    private readonly List<SphereArcSpawner> sphereArcSpawners;
    private readonly ThoughtUIView thoughtViewPrefab;
    private readonly List<NegativeThought> negativThoughts;
    private readonly GameData gameData;
    private readonly PlayerData playerData;

    private ThoughtFactory thoughtFactory;
    private List<ThoughtUIView> viewPool;
    private UniTaskCompletionSource spawnDelaySource;

    public ThoughtSpawner(NegativeThoughtConfig thoughtConfigs, List<SphereArcSpawner> sphereArcSpawners, ThoughtUIView thoughtViewPrefab, GameData gameData, PlayerData playerData)
    {
        this.thoughtConfigs = thoughtConfigs;
        this.sphereArcSpawners = sphereArcSpawners;
        this.thoughtViewPrefab = thoughtViewPrefab;
        this.gameData = gameData;
        this.playerData = playerData;

        negativThoughts = new List<NegativeThought>();
    }

    public void SetFactory(ThoughtFactory thoughtFactory)
    {
        this.thoughtFactory = thoughtFactory;
    }

    public void StartSpawn()
    {
        viewPool = new List<ThoughtUIView>();

        SpawnThought();
    }


    public NegativeThought GetTargetThought()
    {
        return negativThoughts.Count != 0 ? negativThoughts[0] : null;
    }

    public ThoughtUIView GetThoughtView()
    {
        return viewPool[UnityEngine.Random.Range(0, viewPool.Count)];
    }

    public void SpawnThought()
    {
        spawnDelaySource?.TrySetCanceled();
        var config = GetRandomForm();

        NegativeThought newThought = thoughtFactory.GetThought(config);
        negativThoughts.Add(newThought);

        ThoughtUIView thoughtView = GameObject.Instantiate(thoughtViewPrefab);

        newThought.OnDeath += OnDestroyThought;
        newThought.OnHealthChange += thoughtView.Redraw;

        thoughtView.gameObject.SetActive(false);
        viewPool.Add(thoughtView);

        SphereArcSpawner spawnPoint = GetRandomSpawnPoint();
        spawnPoint.OnSpawnCompleted += OnThoughtSpawned;

        thoughtView.Initialize(newThought, spawnPoint);
        thoughtView.Icon.sprite = config.Icon;

        spawnPoint.SpawnAlongArc(thoughtView, config);
    }

    private void OnThoughtSpawned(SphereArcSpawner spawner)
    {
        spawner.OnSpawnCompleted -= OnThoughtSpawned;
        spawner.ThoughtUIView.Thought.IsActive = true;
       
        OnSpawn?.Invoke();
    }

    public async UniTask SetDelayNextSpawn()
    {
        spawnDelaySource = new UniTaskCompletionSource();

        float baseInterval = 30f;
        float minInterval = 3f;
        float decayRate = 0.045f;

        float interval = minInterval + (baseInterval - minInterval) * Mathf.Exp(-decayRate * (playerData.MindLevel + 1));

        float noise = UnityEngine.Random.Range(-0.15f, 0.15f);
        interval *= (1f + noise);

        var delayTask = UniTask.Delay((int)(interval * 1000));
        var controlTask = spawnDelaySource.Task;

        await UniTask.WhenAny(delayTask, controlTask);

        if (controlTask.Status == UniTaskStatus.Canceled)
            return;

        SpawnThought();
    }

    public void DestroyAllThought()
    {
        spawnDelaySource?.TrySetCanceled();

        for (int i = 0; i < negativThoughts.Count;)
        {
            var thought = negativThoughts[0];
            OnDestroyThought(thought);
        }
    }

    private SphereArcSpawner GetRandomSpawnPoint()
    {
        sphereArcSpawners.Shuffle();

        SphereArcSpawner sphereArcSpawner = sphereArcSpawners.FirstOrDefault(view => !view.IsActive);
        return sphereArcSpawner;
    }

    private NegativeThoughtForm GetRandomForm()
    {
        var level = thoughtConfigs.NegativeThoughtLevels[Mathf.Clamp(playerData.MindLevel, 0, int.MaxValue)];

        return level.NegativeThoughtForms[UnityEngine.Random.Range(0, level.NegativeThoughtForms.Count)];
    }

    private void OnDestroyThought(NegativeThought thought)
    {
        ThoughtUIView thoughtView = viewPool.FirstOrDefault(t => t.Thought.Equals(thought));

        thought.OnHealthChange -= thoughtView.Redraw;
        thought.OnDeath -= OnDestroyThought;
        
        negativThoughts.Remove(thought);
        viewPool.Remove(thoughtView);

        thoughtView.SphereArcSpawner.DisableView();

        GameObject.Destroy(thoughtView.gameObject);

        OnDestroy.Invoke();
    }

    private void Unsubscribe()
    {
        int i = 0;
        foreach (var item in negativThoughts)
        {
            item.OnDeath -= OnDestroyThought;
            item.OnHealthChange -= viewPool[i].Redraw;
            i++;
        }
    }

    public void Dispose()
    {
        Unsubscribe();
    }
}

