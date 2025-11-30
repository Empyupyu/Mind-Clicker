public class RewardButtonInitializedSignal
{
    public AdvertisementRewardID RewardId { get; }
    public AdvertisementRewardButton Button { get; }

    public RewardButtonInitializedSignal(AdvertisementRewardID rewardId, AdvertisementRewardButton button)
    {
        RewardId = rewardId;
        Button = button;
    }
}
