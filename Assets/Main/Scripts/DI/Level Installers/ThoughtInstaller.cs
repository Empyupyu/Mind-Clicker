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
        Container.Bind<IThoughtLogic>().To<Tier1EnemyThoughtLogic>().AsTransient().WithArguments(ThoughtType.Tier1Enemy).Lazy();
        Container.Bind<IThoughtLogic>().To<BossBubbleThoughtLogic>().AsTransient().WithArguments(ThoughtType.Boss1).Lazy();
        Container.Bind<IThoughtLogic>().To<BossCemeteryThoughtLogic>().AsTransient().WithArguments(ThoughtType.BossCemetery).Lazy();

        Container.BindInterfacesAndSelfTo<ThoughtFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<ThoughtSpawner>().AsSingle().OnInstantiated<ThoughtSpawner>((ctx, spawner) =>
        {
            spawner.SetFactory(ctx.Container.Resolve<ThoughtFactory>());
        });
    }
}