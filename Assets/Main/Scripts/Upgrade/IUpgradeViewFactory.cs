public interface IUpgradeViewFactory
{
    UpgradeStateView Create(UpgradeConfig config, int index, bool unlocked);
    UpgradeStateView CreateComingSoon(int index);
}
