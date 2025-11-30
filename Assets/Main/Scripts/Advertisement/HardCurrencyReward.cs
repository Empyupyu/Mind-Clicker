public class HardCurrencyReward : IRewardHandler
{
    private readonly int ammount;
    private readonly PlayerDataRef playerDataRef;

    public HardCurrencyReward(int ammount, PlayerDataRef playerDataRef)
    {
        this.ammount = ammount;
        this.playerDataRef = playerDataRef;
    }

    public void Apply()
    {
        playerDataRef.Value.HardCurrency += ammount;
    }
}
