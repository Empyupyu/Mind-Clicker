using YG;

public class YandexAdvertisementProvider : AdvertisementProviderBase
{
    public override void EnableBanner(bool enableBanner)
    {
        YG2.StickyAdActivity(enableBanner);
    }

    public override void ShowInterstitial()
    {
        YG2.InterstitialAdvShow();
    }

    public override void ShowReward(string id)
    {
        YG2.RewardedAdvShow(id);
    }

    protected override void SubscribeEvents()
    {
        YG2.onCloseInterAdv += RaiseInterstitialCompleted;
        YG2.onRewardAdv += RaiseRewardCompleted;
    }

    protected override void UnsubscribeEvents()
    {
        YG2.onCloseInterAdv -= RaiseInterstitialCompleted;
        YG2.onRewardAdv -= RaiseRewardCompleted;
    }
}
