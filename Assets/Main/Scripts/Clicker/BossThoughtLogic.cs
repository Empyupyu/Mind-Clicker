using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class BossThoughtLogic : IThoughtLogic, IDisposable
{
    public ThoughtType ThoughtType { get; }

    private readonly Mind mind;
    private readonly Timer timer;
    private readonly ThoughtSpawner thoughtSpawner;
    private readonly BossView bossView;

    private BossView bossViewInstance;
    private NegativeThought currentNegativeThought;

    public BossThoughtLogic(ThoughtType thoughtType, Mind mind, Timer timer, ThoughtSpawner thoughtSpawner, BossView bossView)
    {
        ThoughtType = thoughtType;
        this.mind = mind;
        this.timer = timer;
        this.thoughtSpawner = thoughtSpawner;
        this.bossView = bossView;
    }

    public void AttachTo(NegativeThought thought)
    {
        thoughtSpawner.DestroyAllThought();

        currentNegativeThought = thought;
        bossViewInstance = GameObject.Instantiate(bossView);

        timer.Initialize();
        timer.StartTimer(60).Forget();

        timer.OnFinished += OnTimerFinished;
        currentNegativeThought.OnDeath += OnBossDeath;
    }

    private void OnBossDeath(NegativeThought negativeThought)
    {
        timer.Disable();
        mind.LevelUp();
        GameObject.Destroy(bossViewInstance.gameObject);
    }

    private void OnTimerFinished()
    {
        mind.ReduceLevel();
        thoughtSpawner.DestroyAllThought();
        thoughtSpawner.SpawnThought();
        timer.Disable();
        GameObject.Destroy(bossViewInstance.gameObject);
    }

    public void Dispose()
    {
        timer.OnFinished -= OnTimerFinished;
        currentNegativeThought.OnDeath -= OnBossDeath;
    }
}
