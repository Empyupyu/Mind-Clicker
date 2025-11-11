using Zenject;

public class PlatformInstaller : MonoInstaller
{
    [Inject] private ProjectSettingsConfig projectSettingsConfig;

    public override void InstallBindings()
    {
        var providerClass = PlayerDataProviderFactory.GetProviderType(projectSettingsConfig.PlayerDataProviderType);
        var advertismentProvider = AdvertismentProviderFactory.GetProviderType(projectSettingsConfig.PlayerDataProviderType);

        Container.Bind(typeof(IPlayerDataProvider)).To(providerClass).AsSingle();
        Container.BindInterfacesAndSelfTo(advertismentProvider).AsSingle();
    }
}
