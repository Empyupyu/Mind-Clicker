public class UpgradeCalculator : IUpgradeCalculator
{
    public float CalculateEffect(UpgradeConfig config, int upgradeLevel)
    {
        return config.BaseEffect + config.EffectMultiplier * upgradeLevel;
    }

    public float CalculatePrice(UpgradeConfig config, int upgradeLevel)
    {
        return config.BasePrice + (upgradeLevel * config.PriceMultiplier);
    }
}