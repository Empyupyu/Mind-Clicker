using System;
using Zenject;

public class UpgradeSaveService : IInitializable, IDisposable
{
    private readonly PlayerDataRef playerData;
    private readonly Upgrade upgrade;
    private readonly SaveLoadService saveLoadService;

    public UpgradeSaveService(PlayerDataRef playerData, Upgrade upgrade, SaveLoadService saveLoadService)
    {
        this.playerData = playerData;
        this.upgrade = upgrade;
        this.saveLoadService = saveLoadService;
    }

    public void Initialize()
    {
        upgrade.OnUpgrade += SaveUpgrade;
    }

    private void SaveUpgrade(UpgradeData upgradeData)
    {
        saveLoadService.Save(playerData.Value);
    }

    public void Dispose()
    {
        upgrade.OnUpgrade -= SaveUpgrade;
    }
}

