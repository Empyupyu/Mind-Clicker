using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class BossFightPrepare : IDisposable
{
    public event Action OnTimerFinished;

    private readonly Mind mind;
    private readonly Timer timer;
    private readonly ThoughtSpawner thoughtSpawner;
    private readonly BossUIView bossUIView;

    private BossUIView bossViewInstance;

    public BossFightPrepare(Mind mind, Timer timer, ThoughtSpawner thoughtSpawner, BossUIView bossUIView)
    {
        this.mind = mind;
        this.timer = timer;
        this.thoughtSpawner = thoughtSpawner;
        this.bossUIView = bossUIView;
    }

    public void Prepare()
    {
        thoughtSpawner.DestroyAll();
        bossViewInstance = GameObject.Instantiate(bossUIView);
    }

    //TODO
    public void StartFight()
    {
        timer.Initialize();
        timer.StartTimer(60).Forget();

        timer.OnFinished += TimerFinished;
    }

    private void TimerFinished()
    {
        mind.ReduceLevel();
        thoughtSpawner.DestroyAll();
        thoughtSpawner.Spawn();
        timer.Disable();
        RemoveBossView();

        OnTimerFinished?.Invoke();
    }

    public void OnBossDeath(NegativeThought negativeThought)
    {
        timer.Disable();
        mind.LevelUp();
        RemoveBossView();
    }

    private void RemoveBossView()
    {
        GameObject.Destroy(bossViewInstance.gameObject);
    }

    public void Dispose()
    {
        timer.OnFinished -= OnTimerFinished;
    }
}