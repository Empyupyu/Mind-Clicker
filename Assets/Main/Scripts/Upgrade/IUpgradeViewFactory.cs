public interface IUpgradeViewFactory
{
    UpgradeStateView Create(IUpgradeEffect effect, int index, bool unlocked);
    UpgradeStateView CreateComingSoon(int index);
}
