public class RewardCooldownUpdatedSignal
{
    public AdvertisementRewardID RewardId { get; }
    public bool IsAvailable { get; }

    public RewardCooldownUpdatedSignal(AdvertisementRewardID rewardId, bool isAvailable)
    {
        RewardId = rewardId;
        IsAvailable = isAvailable;
    }
}
