using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

public class BossFightPrepare : IDisposable
{
    public event Action OnTimerFinished;

    private TimerView timerViewInstance;
    private BossUIView bossViewInstance;

    private readonly Timer timer;
    private readonly BossUIView bossUIView;
    private readonly TimerView timerView;
    private readonly BossFightData bossFightData;
    private readonly SignalBus signalBus;

    public BossFightPrepare(
        Timer timer,
        BossUIView bossUIView,
        TimerView timerView,
        BossFightData bossFightData,
        SignalBus signalBus)
    {
        this.timer = timer;
        this.bossUIView = bossUIView;
        this.timerView = timerView;
        this.bossFightData = bossFightData;
        this.signalBus = signalBus;
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

        signalBus.Subscribe<PrestigeSignal>(CleanUp);
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

    private void CleanUp()
    {
        timer.Disable();
        RemoveBossUIView();
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
        signalBus.Unsubscribe<PrestigeSignal>(CleanUp);
    }

    public void Dispose()
    {
        timer.OnFinished -= OnTimerFinished;
        signalBus.TryUnsubscribe<PrestigeSignal>(CleanUp);
    }
}