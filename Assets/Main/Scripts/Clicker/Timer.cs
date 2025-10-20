using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class Timer
{
    public event Action OnFinished;

    private readonly TimerView timerViewPrefab;
    private TimerView timerView;
    private float currentDuration;
    private bool isPaused;

    public Timer(TimerView timerView)
    {
        this.timerViewPrefab = timerView;
    }

    public void Initialize()
    {
        timerView = GameObject.Instantiate(timerViewPrefab);
    }

    public void Pause(bool enable)
    {
        isPaused = enable;
    }

    public void Disable()
    {
        GameObject.Destroy(timerView.gameObject);
        currentDuration = 0;
        OnFinished = null;
    }

    public async UniTask StartTimer(float duration)
    {
        currentDuration = duration;

        while (currentDuration > 0)
        {
            await UniTask.Yield();

            if (isPaused) continue;

            currentDuration -= Time.deltaTime;
            currentDuration = Math.Clamp(currentDuration, 0, float.MaxValue);
            timerView.TimerText.text = string.Format("{0:0.0}", currentDuration);
        }

        OnFinished?.Invoke();
    }
}
