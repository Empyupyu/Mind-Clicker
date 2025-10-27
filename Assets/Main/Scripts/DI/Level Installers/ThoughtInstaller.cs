using Zenject;

public class ThoughtInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<BossFightPrepare>().AsTransient();
        Container.Bind<IThoughtHealthProvider>().To<ThoughtStandartHealthProvider>().AsSingle();
        Container.Bind<IThoughtLogic>().To<Tier1EnemyThoughtLogic>().AsTransient().WithArguments(ThoughtType.Tier1Enemy);
        Container.Bind<IThoughtLogic>().To<BossBubbleThoughtLogic>().AsTransient().WithArguments(ThoughtType.Boss1);
        Container.Bind<IThoughtLogic>().To<BossCemeteryThoughtLogic>().AsTransient().WithArguments(ThoughtType.BossCemetery);

        Container.BindInterfacesAndSelfTo<ThoughtFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<ThoughtSpawner>().AsSingle().OnInstantiated<ThoughtSpawner>((ctx, spawner) =>
        {
            spawner.SetFactory(ctx.Container.Resolve<ThoughtFactory>());
        });
    }
}