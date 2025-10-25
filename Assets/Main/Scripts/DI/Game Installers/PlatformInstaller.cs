using Zenject;

public class PlatformInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IPlayerDataProvider>().To<YandexCloudDataProvider>().AsSingle();
    }
}
