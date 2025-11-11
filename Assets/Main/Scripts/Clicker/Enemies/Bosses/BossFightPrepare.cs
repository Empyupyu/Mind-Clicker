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
    private readonly TimerView timerView;
    private TimerView timerViewInstance;
    private BossUIView bossViewInstance;

    public BossFightPrepare(Mind mind, Timer timer, ThoughtSpawner thoughtSpawner, BossUIView bossUIView, TimerView timerView)
    {
        this.mind = mind;
        this.timer = timer;
        this.thoughtSpawner = thoughtSpawner;
        this.bossUIView = bossUIView;
        this.timerView = timerView;
    }

    public void Prepare()
    {
        thoughtSpawner.DestroyAll();
        bossViewInstance = GameObject.Instantiate(bossUIView);
    }

    public void StartFight()
    {
        timerViewInstance = GameObject.Instantiate(timerView);
        timer.OnTick += RedrawView;
        timer.StartTimer(60).Forget();

        timer.OnFinished += TimerFinished;
    }

    private void RedrawView(float time)
    {
        timerViewInstance.Redraw(string.Format("{0:0.0}", time));
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
        GameObject.Destroy(timerViewInstance.gameObject);
    }

    public void Dispose()
    {
        timer.OnFinished -= OnTimerFinished;
    }
}