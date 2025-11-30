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

        Container.Bind<IThoughtHandler>().To<Tier1EnemyThoughtHandler>().AsTransient().WithArguments(ThoughtType.Tier1Enemy);
        Container.Bind<IThoughtHandler>().To<BossBubbleThoughtHandler>().AsTransient().WithArguments(ThoughtType.Boss1);
        Container.Bind<IThoughtHandler>().To<BossCemeteryThoughtHandler>().AsTransient().WithArguments(ThoughtType.BossCemetery);
        Container.Bind<IThoughtHandler>().To<BossPhoneThoughtHandler>().AsTransient().WithArguments(ThoughtType.BossPhone);

        Container.BindInterfacesAndSelfTo<ThoughtFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<ThoughtSpawner>().AsSingle();
        //    .OnInstantiated<ThoughtSpawner>((ctx, spawner) =>
        //{
        //    spawner.SetFactory(ctx.Container.Resolve<ThoughtFactory>());
        //});
    }
}