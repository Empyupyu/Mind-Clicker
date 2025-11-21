using TMPro;
using Zenject;

public class YandexInstaller : Installer<YandexInstaller>
{
    private ProjectSettingsConfig projectSettingsConfig;
    private ModulePrioritiesConfig modulePrioritiesConfig;

    public YandexInstaller(ProjectSettingsConfig config, ModulePrioritiesConfig modulePrioritiesConfig)
    {
        this.projectSettingsConfig = config;
        this.modulePrioritiesConfig = modulePrioritiesConfig;
    }

    public override void InstallBindings()
    {
        Container.Bind<IEnvironmentProvider>().To<YandexEnvironmentProvider>().AsSingle();

        BindAdvertisement();
        BindAuthorization();
        BindGameModules();
    }

    private void BindAdvertisement()
    {
        var advertismentProvider = AdvertismentProviderFactory.GetProviderType(projectSettingsConfig.PlayerDataProviderType);
        Container.BindInterfacesAndSelfTo(advertismentProvider).AsSingle();
        
        Container.BindInterfacesAndSelfTo<AdvertisementService>().AsSingle();
        Container.Bind<AdvertisementRewardService>().AsSingle().WithArguments(modulePrioritiesConfig.AdvertisementReward);
        Container.Bind<AdvertisementRewardRegistry>().AsSingle();
        Container.Bind<AdvertisementRewardCooldownService>().AsSingle();

        BindRewardHandlers();
        BindInterstitialTriggers();
    }

    private void BindInterstitialTriggers()
    {
        Container.Bind<IAdvertisementTrigger>().To<TimerAdvertisementTrigger>()
                  .AsSingle();
    }

    private void BindRewardHandlers()
    {
        Container.Bind<IRewardHandler>()
                    .To<HardCurrencyReward>()
                    .AsTransient()
                    .WithArguments(100);

        Container.Bind<IRewardHandler>()
            .To<DisabledAdvertisementReward>()
            .AsTransient();
    }

    private void BindGameModules()
    {
        Container.Bind<IGameModule>().To<AuthorizationModule>().AsSingle().WithArguments(modulePrioritiesConfig.Authorization);
        Container.BindInterfacesAndSelfTo<AdvertisementController>().AsSingle().WithArguments(modulePrioritiesConfig.Advertisement);
    }

    private void BindAuthorization()
    {
        Container.Bind<AuthorizationService>().AsSingle();
        Container.Bind<IAuthorizationProvider>().To<YandexAuthorizationProvider>().AsSingle();
    }
}
