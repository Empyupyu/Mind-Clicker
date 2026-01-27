using Zenject;

public class LevelCoreInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<LevelLoop>().AsSingle().Lazy();
        Container.BindInterfacesAndSelfTo<PrestigeService>().AsSingle();
        Container.Bind<MindProgress>().AsSingle();
        Container.Bind<LightService>().AsSingle();
        Container.BindInterfacesAndSelfTo<ThoughtDamageService>().AsSingle();
        Container.BindInterfacesAndSelfTo<DamageFeedbackService>().AsSingle();
        Container.Bind<IMindProgressUpdater>().To<MindProgressUpdater>().AsSingle();
        Container.Bind<IMindLevelAnimator>().To<MindLevelAnimator>().AsSingle();
        Container.Bind<IPristigeCalculate>().To<SqrtPristigeCalculate>().AsSingle();
        Container.BindInterfacesAndSelfTo<MindLevelPresentation>().AsSingle();
        Container.BindInterfacesAndSelfTo<MindController>().AsSingle();
        Container.Bind<Timer>().AsSingle();
        Container.Bind<MoneyWallet>().AsSingle();
        Container.BindInterfacesAndSelfTo<MoneyController>().AsSingle().Lazy();
        Container.BindInterfacesAndSelfTo<ThoughtRewardHandler>().AsSingle();
    }
}
