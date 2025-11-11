using System;
using Zenject;
using Action = System.Action;

public interface IAdvertisementProvider
{
    public event Action OnInterstitialCompleted;
    public event Action<string> OnRewardCompleted;

    public void EnableBanner(bool enableBanner);
    public void ShowInterstitial();
    public void ShowReward(string id);
}

public abstract class AdvertisementProviderBase : IAdvertisementProvider, IInitializable, IDisposable
{
    public event Action OnInterstitialCompleted;
    public event Action<string> OnRewardCompleted;

    public void Initialize()
    {
        SubscribeEvents();
    }

    public void Dispose()
    {
        UnsubscribeEvents();
    }

    public abstract void EnableBanner(bool enableBanner);
    public abstract void ShowInterstitial();
    public abstract void ShowReward(string id);

    protected abstract void SubscribeEvents();
    protected abstract void UnsubscribeEvents();

    protected void RaiseInterstitialCompleted() => OnInterstitialCompleted?.Invoke();
    protected void RaiseRewardCompleted(string id) => OnRewardCompleted?.Invoke(id);
}
