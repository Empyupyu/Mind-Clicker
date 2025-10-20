public class UpgradeData
{
    public readonly UpgradeProgress UpgradeProgress;
    public readonly IUpgradeEffect Effect;
    public float Price { get; private set; }
    public float CurrentLevelBonusEffect { get; private set; }
    public float NextLevelBonusEffect { get; private set; }

    public UpgradeData(UpgradeProgress upgradeProgress, IUpgradeEffect upgradeEffect, float price,
        float currentLevelBonusEffect, float nextLevelBonusEffect)
    {
        UpgradeProgress = upgradeProgress;
        Effect = upgradeEffect;
        Price = price;
        CurrentLevelBonusEffect = currentLevelBonusEffect;
        NextLevelBonusEffect = nextLevelBonusEffect;
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
}
