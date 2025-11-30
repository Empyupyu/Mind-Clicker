using System;
using System.Collections.Generic;
using Zenject;

public class AnalyticsMediator : IInitializable, IDisposable
{
    private readonly IAnalytics analyticsService;
    private readonly SignalBus signalBus;
    private readonly GameData gameData;
    private UpgradeService upgradeService;

    public AnalyticsMediator(
        IAnalytics analyticsService,
        SignalBus signalBus,
        GameData gameData)
    {
        this.analyticsService = analyticsService;
        this.signalBus = signalBus;
        this.gameData = gameData;
    }

    public void Initialize()
    {
        signalBus.Subscribe<GameLoadedSignal>(RegisterAnalyticsEvents);
    }

    public void Dispose()
    {
        upgradeService.OnUpgrade -= OnUpgrade;
    }

    private void RegisterAnalyticsEvents()
    {
        upgradeService = gameData.Level.Container.Resolve<UpgradeService>();

        upgradeService.OnUpgrade += OnUpgrade;
    }

    private void OnUpgrade(Upgrade upgrade)
    {
        analyticsService.Send("upgrade_", new Dictionary<string, object>() 
        { { upgrade.Config.Type.ToString(), upgrade.UpgradeProgress.Level } });
    }
}