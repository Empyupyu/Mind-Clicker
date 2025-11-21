using System;
using Action = System.Action;

public interface IAdvertisementProvider
{
    public event Action OnInterstitialCompleted;
    public event Action<string> OnRewardCompleted;

    public void EnableBanner(bool enableBanner);
    public void ShowInterstitial();
    public void ShowReward(string id);
}
