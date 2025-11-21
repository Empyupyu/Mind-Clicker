using System;
using Zenject;

public class AdvertisementService : IInitializable, IDisposable
{
    public event Action OnInterstitialCompleted;
    public event Action<string> OnRewardCompleted;

    private readonly IAdvertisementProvider advertisementProvider;

    public AdvertisementService(IAdvertisementProvider provider)
    {
        advertisementProvider = provider;
    }

    public void Initialize()
    {
        advertisementProvider.OnInterstitialCompleted += RaiseInterstitialCompleted;
        advertisementProvider.OnRewardCompleted += RaiseRewardCompleted;
    }

    private void RaiseInterstitialCompleted()
    {
        OnInterstitialCompleted?.Invoke();
    }

    private void RaiseRewardCompleted(string id)
    {
        OnRewardCompleted.Invoke(id);
    }

    public void EnableBanner(bool isActive)
    {
        advertisementProvider.EnableBanner(isActive); 
    }

    public void ShowInterstitial()
    {
        advertisementProvider.ShowInterstitial();
    }

    public void ShowReward(string id)
    {
        advertisementProvider.ShowReward(id);
    }

    public void Dispose()
    {
        advertisementProvider.OnInterstitialCompleted -= RaiseInterstitialCompleted;
        advertisementProvider.OnRewardCompleted -= RaiseRewardCompleted;
    }
}
