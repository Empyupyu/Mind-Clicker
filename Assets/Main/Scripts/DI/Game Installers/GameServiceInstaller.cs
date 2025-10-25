using Zenject;

public class GameServiceInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlayerData>().AsSingle();
        Container.Bind<PlayerDataRef>().AsSingle();
        Container.Bind<GameData>().AsSingle();

        Container.Bind<SaveLoadService>().AsSingle();
        Container.Bind<AddressableAssetLoader>().AsSingle();

        Container.Bind<GameFlowController>().FromNewComponentOnNewGameObject()
            .WithGameObjectName("GameFlowController")
            .AsSingle()
            .NonLazy();
    }
}
