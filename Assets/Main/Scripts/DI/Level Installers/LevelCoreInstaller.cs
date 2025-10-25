using Zenject;

public class LevelCoreInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Mind>().AsSingle();
        Container.Bind<UpgradeMaterialAnimation>().AsSingle();
        Container.BindInterfacesAndSelfTo<DealDamage>().AsSingle();
        Container.BindInterfacesAndSelfTo<DamageFeedbackService>().AsSingle();
        Container.BindInterfacesAndSelfTo<MindController>().AsSingle();
        Container.Bind<Timer>().AsSingle();
        Container.Bind<MoneyWallet>().AsSingle();
        Container.BindInterfacesAndSelfTo<MoneyController>().AsSingle();
        Container.BindInterfacesAndSelfTo<ThoughtRewardHandler>().AsSingle();
    }
}
