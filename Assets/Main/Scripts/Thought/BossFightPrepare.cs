using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class BossFightPrepare : IDisposable
{
    public event Action OnTimerFinished;

    private readonly Timer timer;
    private readonly BossUIView bossUIView;
    private readonly TimerView timerView;
    private readonly BossFightData bossFightData;
    private TimerView timerViewInstance;
    private BossUIView bossViewInstance;

    public BossFightPrepare(
        Timer timer,
        BossUIView bossUIView,
        TimerView timerView,
        BossFightData bossFightData)
    {
        this.timer = timer;
        this.bossUIView = bossUIView;
        this.timerView = timerView;
        this.bossFightData = bossFightData;
    }

    public void Prepare()
    {
        bossViewInstance = GameObject.Instantiate(bossUIView);
    }

    public void StartFight()
    {
        timerViewInstance = GameObject.Instantiate(timerView);
        timer.OnTick += RedrawView;
        timer.StartTimer(bossFightData.Duration).Forget();

        timer.OnFinished += TimerFinished;
    }

    private void RedrawView(float time)
    {
        timerViewInstance.Redraw(string.Format("{0:0.0}", time));
    }

    private void TimerFinished()
    {
        timer.Disable();
        RemoveBossUIView();

        OnTimerFinished?.Invoke();
    }

    public void OnBossDeath(NegativeThought negativeThought)
    {
        timer.Disable();
        RemoveBossUIView();
    }

    private void RemoveBossUIView()
    {
        GameObject.Destroy(bossViewInstance.gameObject);
        GameObject.Destroy(timerViewInstance.gameObject);
    }

    public void Dispose()
    {
        timer.OnFinished -= OnTimerFinished;
    }
}