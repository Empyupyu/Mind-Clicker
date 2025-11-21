using Zenject;

public class PlatformInstaller : MonoInstaller
{
    [Inject] private ProjectSettingsConfig projectSettingsConfig;

    public override void InstallBindings()
    {
        var providerClass = PlayerDataProviderFactory.GetProviderType(projectSettingsConfig.PlayerDataProviderType);
        Container.Bind(typeof(IPlayerDataProvider)).To(providerClass).AsSingle();

        switch (projectSettingsConfig.PlayerDataProviderType)
        {
            case PlayerDataProviderType.Yandex:
                YandexInstaller.Install(Container);
                break;

            case PlayerDataProviderType.Steam:
                break;

            case PlayerDataProviderType.Local:
                LocalInstaller.Install(Container);
                break;
        }
    }
}
