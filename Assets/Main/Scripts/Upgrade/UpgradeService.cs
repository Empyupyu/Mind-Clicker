using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

public class UpgradeService : IInitializable
{
    public event Action<Upgrade> OnUpgrade;
    public Dictionary<string, Upgrade> EffectsData => effectsData;

    private readonly MoneyWallet wallet;
    private readonly List<IUpgradeEffect> effects;
    private readonly PlayerDataRef playerData;
    private readonly List<UpgradeConfig> upgradeConfigs;
    private readonly IUpgradeCalculator upgradeCalcalator;
    private readonly Dictionary<string, Upgrade> effectsData = new();

    public UpgradeService(
        MoneyWallet wallet,
        List<IUpgradeEffect> effects,
        PlayerDataRef playerData,
        List<UpgradeConfig> upgradeConfigs,
        IUpgradeCalculator upgradeCalcalator)
    {
        this.wallet = wallet;
        this.effects = effects;
        this.playerData = playerData;
        this.upgradeConfigs = upgradeConfigs;
        this.upgradeCalcalator = upgradeCalcalator;
    }

    public void Initialize()
    {
        InitializeUpgrades();
        ApplyActiveUpgrades();
    }

    private void InitializeUpgrades()
    {
        if (playerData.Value.Upgrades == null)
        {
            playerData.Value.Upgrades = new List<UpgradeProgress>();
        }

        foreach (var config in upgradeConfigs)
        {
            effectsData[config.Title] = CreateUpgrade(config);
        }
    }

    private void ApplyActiveUpgrades()
    {
        foreach (var upgradeProgress in playerData.Value.Upgrades)
        {
            Upgrade upgrade = effectsData[upgradeProgress.ID];

            if (upgrade.UpgradeProgress.Level == 0) continue;

            upgrade.EffectStrategy.Apply(upgrade.Config, upgradeProgress.Level);
        }
    }

    public void Buy(string upgradeType)
    {
        Upgrade upgrade = effectsData[upgradeType];

        if (!wallet.HasEnough(upgrade.Price)) return;
        wallet.Spend(upgrade.Price);

        upgrade.UpgradeProgress.Level++;
        upgrade.SetLevelBonuseEffect(CalculateEffect(upgrade.Config, upgrade.UpgradeProgress.Level + 1));
        upgrade.SetNextLevelBonuseEffect(CalculateEffect(upgrade.Config, upgrade.UpgradeProgress.Level + 2));
        upgrade.SetPrice(CalculatePrice(upgrade.Config, upgrade.UpgradeProgress.Level + 1));
        upgrade.EffectStrategy.Apply(upgrade.Config, upgrade.UpgradeProgress.Level);

        OnUpgrade?.Invoke(upgrade);
    }

    private Upgrade CreateUpgrade(UpgradeConfig config)
    {
        UpgradeProgress progress = GetUpgrade(config.Title);

        return new Upgrade(progress, config,
             CalculatePrice(config, progress.Level),
            CalculateEffect(config, progress.Level + 1),
            CalculateEffect(config, progress.Level + 2),
            GetUpgradeEffect(config.Type));
    }

    public UpgradeProgress GetUpgrade(string typeName)
    {
        UpgradeProgress upgradeProgress = playerData.Value.Upgrades.FirstOrDefault(upgrade => upgrade.ID.Equals(typeName));

        if(upgradeProgress == null)
        {
            upgradeProgress = new UpgradeProgress();
            upgradeProgress.ID = typeName;
            playerData.Value.Upgrades.Add(upgradeProgress);
        }

        return upgradeProgress;
    }

    private float CalculateEffect(UpgradeConfig config, int upgradeLevel)
    {
        return upgradeCalcalator.CalculateEffect(config, upgradeLevel);
    }

    private float CalculatePrice(UpgradeConfig config, int upgradeLevel)
    {
        return upgradeCalcalator.CalculatePrice(config, upgradeLevel);
    }

    private IUpgradeEffect GetUpgradeEffect(UpgradeType typeName)
    {
        return effects.FirstOrDefault(u => u.Type == typeName);
    }
}
