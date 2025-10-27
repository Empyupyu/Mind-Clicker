using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using static UnityEngine.InputManagerEntry;

public class BossFightPrepare : IDisposable
{
    public event Action OnTimerFinished;

    private readonly Mind mind;
    private readonly Timer timer;
    private readonly ThoughtSpawner thoughtSpawner;
    private readonly BossView bossView;

    private BossView bossViewInstance;

    public BossFightPrepare(Mind mind, Timer timer, ThoughtSpawner thoughtSpawner, BossView bossView)
    {
        this.mind = mind;
        this.timer = timer;
        this.thoughtSpawner = thoughtSpawner;
        this.bossView = bossView;
    }

    public void Prepare()
    {
        thoughtSpawner.DestroyAll();
        bossViewInstance = GameObject.Instantiate(bossView);
    }

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
        GameObject.Destroy(bossViewInstance.gameObject);

        OnTimerFinished?.Invoke();
    }

    public void OnBossDeath(NegativeThought negativeThought)
    {
        timer.Disable();
        mind.LevelUp();
        GameObject.Destroy(bossViewInstance.gameObject);
    }

    public void Dispose()
    {
        timer.OnFinished -= OnTimerFinished;
    }
}