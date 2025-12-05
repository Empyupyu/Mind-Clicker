using Zenject;

public class ThoughtInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<BossFightPrepare>().AsTransient();
        Container.Bind<IThoughtHealthProvider>().To<ThoughtStandartHealthProvider>().AsSingle();
        Container.Bind<IThoughtLifecycleService>().To<ThoughtLifecycleService>().AsSingle();
        Container.Bind<ISpawnTimingCalculator>().To<SpawnTimingCalculator>().AsSingle();
        Container.Bind<IThoughtViewPool>().To<ThoughtViewPool>().AsSingle();
        Container.Bind<ISpawnPointSelector>().To<SpawnPointSelector>().AsSingle();
        Container.Bind<IThoughtFormSelector>().To<ThoughtFormSelector>().AsSingle();
        Container.Bind<ISphereArcBuilder>().To<SphereArcBuilder>().AsSingle();
        Container.Bind<ISphereArcAnimator>().To<SphereArcAnimator>().AsSingle();
        Container.Bind<BossEnvironmentRegistry>().AsSingle();
        Container.Bind<SphereArcSpawner>().AsSingle();
        Container.BindInterfacesAndSelfTo<BossEnvironmentController>().AsSingle();

        Container.Bind<ILevelStrategy>().To<Tier1EnemyLevelStrategy>().AsTransient().WithArguments(ThoughtType.Tier1Enemy);
        Container.Bind<ILevelStrategy>().To<BossBubbleLevelStrategy>().AsTransient().WithArguments(ThoughtType.Boss1);
        Container.Bind<ILevelStrategy>().To<BossCemeteryLevelStrategy>().AsTransient().WithArguments(ThoughtType.BossCemetery);
        Container.Bind<ILevelStrategy>().To<BossPhoneLevelStrategy>().AsTransient().WithArguments(ThoughtType.BossPhone);

        Container.BindInterfacesAndSelfTo<ThoughtFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<ThoughtSpawner>().AsSingle();
    }
}