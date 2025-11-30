public class DisabledAdvertisementReward : IRewardHandler
{
    private readonly PlayerDataRef playerDataRef;

    public DisabledAdvertisementReward(PlayerDataRef playerDataRef)
    {
        this.playerDataRef = playerDataRef;
    }

    public void Apply()
    {
        playerDataRef.Value.AdvertisementIsDisabled = true;
    }
}
