public class Upgrade
{
    public float Price { get; private set; }
    public float CurrentLevelBonusEffect { get; private set; }
    public float NextLevelBonusEffect { get; private set; }
    public bool IsUnlock { get; private set; }

    public readonly UpgradeProgress UpgradeProgress;
    public readonly UpgradeConfig Config;
    public readonly IUpgradeEffect EffectStrategy;

    public Upgrade(UpgradeProgress upgradeProgress, UpgradeConfig config, float price,
        float currentLevelBonusEffect, float nextLevelBonusEffect, IUpgradeEffect effectStrategy)
    {
        UpgradeProgress = upgradeProgress;
        Config = config;
        Price = price;
        CurrentLevelBonusEffect = currentLevelBonusEffect;
        NextLevelBonusEffect = nextLevelBonusEffect;
        EffectStrategy = effectStrategy;
    }

    public void SetPrice(float price)
    {
        Price = price;
    }

    public void SetLevelBonuseEffect(float effect)
    {
        CurrentLevelBonusEffect = effect;
    }

    public void SetNextLevelBonuseEffect(float effect)
    {
        NextLevelBonusEffect = effect;
    }

    public void SetAvailable(bool available)
    {
        IsUnlock = available;
    }
}
