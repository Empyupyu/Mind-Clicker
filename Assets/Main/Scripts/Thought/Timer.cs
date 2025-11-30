using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class Timer
{
    public event Action OnFinished;
    public event Action<float> OnTick;

    private float currentDuration;
    private bool isPaused;

    public void Pause(bool enable)
    {
        isPaused = enable;
    }

    public void Disable()
    {
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
            OnTick?.Invoke(currentDuration);
        }

        OnFinished?.Invoke();
    }
}
