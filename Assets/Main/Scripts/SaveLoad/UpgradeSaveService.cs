using System;
using Zenject;

public class UpgradeSaveService : IInitializable, IDisposable
{
    private readonly PlayerDataRef playerData;
    private readonly UpgradeService upgradeService;
    private readonly SaveLoadService saveLoadService;

    public UpgradeSaveService(
        PlayerDataRef playerData,
        UpgradeService upgradeService,
        SaveLoadService saveLoadService)
    {
        this.playerData = playerData;
        this.upgradeService = upgradeService;
        this.saveLoadService = saveLoadService;
    }

    public void Initialize()
    {
        upgradeService.OnUpgrade += SaveUpgrade;
    }

    public void Dispose()
    {
        upgradeService.OnUpgrade -= SaveUpgrade;
    }

    private void SaveUpgrade(Upgrade upgrade)
    {
        saveLoadService.Save(playerData.Value);
    }
}

public class MindLevelSaveService : IInitializable, IDisposable
{
    private readonly PlayerDataRef playerData;
    private readonly MindProgress mindProgress;
    private readonly SaveLoadService saveLoadService;

    public MindLevelSaveService(
        PlayerDataRef playerData,
        MindProgress mindProgress,
        SaveLoadService saveLoadService)
    {
        this.playerData = playerData;
        this.mindProgress = mindProgress;
        this.saveLoadService = saveLoadService;
    }

    public void Initialize()
    {
        mindProgress.OnLevelUp += SaveUpgrade;
    }

    public void Dispose()
    {
        mindProgress.OnLevelUp -= SaveUpgrade;
    }

    private void SaveUpgrade()
    {
        saveLoadService.Save(playerData.Value);
    }
}