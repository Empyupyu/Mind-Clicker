using Zenject;

public class GameServiceInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlayerData>().AsSingle();
        Container.Bind<PlayerDataRef>().AsSingle();
        Container.Bind<GameData>().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelMusicFlow>().AsSingle();
        Container.Bind<IModuleInitializer>().To<ModuleInitializer>().AsSingle();
        Container.Bind<ILevelLoader>().To<LevelLoader>().AsSingle();
      
        var priorities = Container.Resolve<ModulePrioritiesConfig>();
        Container.BindInterfacesAndSelfTo<SaveLoadService>().AsSingle().WithArguments(priorities.SaveLoad);

        Container.Bind<AddressableAssetLoader>().AsSingle();
    }
}
