using Zenject;

public class GameStateInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<LoadingLevelState>().AsSingle();
        Container.Bind<LevelState>().AsSingle();
        Container.Bind<InitializeState>().AsSingle();

        Container.Bind<GameStateMachine>().AsSingle()
            .OnInstantiated<GameStateMachine>((ctx, machine) =>
            {
                machine.Register<LoadingLevelState>(ctx.Container.Resolve<LoadingLevelState>());
                machine.Register<LevelState>(ctx.Container.Resolve<LevelState>());
                machine.Register<InitializeState>(ctx.Container.Resolve<InitializeState>());
            });
    }
}
