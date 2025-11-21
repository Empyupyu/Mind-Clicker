using Zenject;

public class LevelCoreInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameLoop>().AsSingle().Lazy();
        Container.Bind<Mind>().AsSingle();
        Container.Bind<LightService>().AsSingle();
        Container.BindInterfacesAndSelfTo<DealDamage>().AsSingle();
        Container.BindInterfacesAndSelfTo<DamageFeedbackService>().AsSingle();
        Container.Bind<IMindProgressService>().To<MindProgressService>().AsSingle();
        Container.BindInterfacesAndSelfTo<MindLevelService>().AsSingle();
        Container.BindInterfacesAndSelfTo<MindController>().AsSingle();
        Container.Bind<Timer>().AsSingle();
        Container.Bind<MoneyWallet>().AsSingle();
        Container.BindInterfacesAndSelfTo<MoneyController>().AsSingle().Lazy();
        Container.BindInterfacesAndSelfTo<ThoughtRewardHandler>().AsSingle();
    }
}
