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

        BindAuthorization();
        BindAnalytics();
        BindAdvertisement();
    }

    private void BindAdvertisement()
    {
        var advertismentProvider = AdvertismentProviderFactory.GetProviderType(projectSettingsConfig.PlayerDataProviderType);
        Container.BindInterfacesAndSelfTo(advertismentProvider).AsSingle();

        Container.BindInterfacesAndSelfTo<AdvertisementService>().AsSingle();
        Container.Bind<AdvertisementRewardService>().AsSingle().WithArguments(modulePrioritiesConfig.AdvertisementReward);
        Container.Bind<AdvertisementRewardRegistry>().AsSingle();
        Container.Bind<AdvertisementRewardCooldownService>().AsSingle();
        Container.BindInterfacesAndSelfTo<AdvertisementController>().AsSingle().WithArguments(modulePrioritiesConfig.Advertisement);

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

    private void BindAnalytics()
    {
        Container.BindInterfacesAndSelfTo<AnalyticsMediator>().AsSingle();
        Container.Bind<IAnalyticsProvider>().To<YandexMetricaAnalyticsProvider>().AsSingle();
        Container.Bind<IAnalytics>().To<AnalyticsService>().AsSingle();
        Container.Bind<IGameModule>().To<AnalyticsModule>().AsSingle().WithArguments(modulePrioritiesConfig.Analytics);
    }

    private void BindAuthorization()
    {
        Container.Bind<AuthorizationService>().AsSingle();
        Container.Bind<IAuthorizationProvider>().To<YandexAuthorizationProvider>().AsSingle();
        Container.Bind<IGameModule>().To<AuthorizationModule>().AsSingle().WithArguments(modulePrioritiesConfig.Authorization);
    }
}
