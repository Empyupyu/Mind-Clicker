public interface IUpgradeCalculator
{
    public float CalculateEffect(UpgradeConfig config, int upgradeLevel);
    public float CalculatePrice(UpgradeConfig config, int upgradeLevel);
}
