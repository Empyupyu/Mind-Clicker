public interface IUpgradeEffect
{
    public UpgradeType Type { get; }
    void Apply(UpgradeConfig config, int level);
}
