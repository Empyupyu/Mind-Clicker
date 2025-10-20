public interface IUpgradeEffect
{
    UpgradeConfig UpgradeConfig { get; }
    void Apply(int level);
}
