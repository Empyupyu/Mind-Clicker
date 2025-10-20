using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

public class Upgrade : IInitializable
{
    public event Action<UpgradeData> OnUpgrade;
    public Dictionary<string, UpgradeData> EffectsData => effectsData;

    private readonly MoneyWallet wallet;
    private readonly List<IUpgradeEffect> effects;
    private readonly PlayerData playerData;
    private readonly Dictionary<string, UpgradeData> effectsData = new();

    public Upgrade(MoneyWallet wallet, List<IUpgradeEffect> effects, PlayerData playerData)
    {
        this.wallet = wallet;
        this.effects = effects;
        this.playerData = playerData;
    }

    public void Initialize()
    {
        InitializeUpgrades();
        ApplyActiveUpgrades();
    }

    private void InitializeUpgrades()
    {
        if (playerData.Upgrades == null)
        {
            playerData.Upgrades = new List<UpgradeProgress>();
        }

        foreach (var effect in effects)
        {
            effectsData[GetEffectByTypeAsString(effect)] = CreateUpgradeData(effect);
        }
    }

    private string GetEffectByTypeAsString(IUpgradeEffect effect)
    {
        return effect.GetType().Name;
    }

    private void ApplyActiveUpgrades()
    {
        foreach (var upgrade in playerData.Upgrades)
        {
            UpgradeData upgradeData = effectsData[upgrade.ID];

            if (upgradeData.Level == 0) continue;

            upgradeData.Effect.Apply(upgrade.Level);
        }
    }

    public void Buy(string upgradeType)
    {
        UpgradeData upgradeData = effectsData[upgradeType];

        if (!wallet.HasEnough(upgradeData.Price)) return;
        wallet.Spend(upgradeData.Price);

        upgradeData.SetLevel(upgradeData.Level + 1);
        upgradeData.SetLevelBonuseEffect(CalculateBonusEffect(upgradeData.Effect, upgradeData.Level + 1));
        upgradeData.SetNextLevelBonuseEffect(CalculateBonusEffect(upgradeData.Effect, upgradeData.Level + 2));
        upgradeData.SetPrice(CalculatePrice(upgradeData.Effect, upgradeData.Level + 1));
        upgradeData.Effect.Apply(upgradeData.Level);

        OnUpgrade?.Invoke(upgradeData);
    }

    private UpgradeData CreateUpgradeData(IUpgradeEffect effect)
    {
        UpgradeProgress upgrade = GetUpgrade(GetEffectByTypeAsString(effect));

        return new UpgradeData(upgrade, effect,
            CalculatePrice(effect, upgrade.Level),
            CalculateBonusEffect(effect, upgrade.Level + 1),
            CalculateBonusEffect(effect, upgrade.Level + 2), upgrade.Level);
    }

    public UpgradeProgress GetUpgrade(string typeName)
    {
        UpgradeProgress upgradeProgress = playerData.Upgrades.FirstOrDefault(upgrade => upgrade.ID.Equals(typeName));

        if(upgradeProgress == null)
        {
            upgradeProgress = new UpgradeProgress();
            upgradeProgress.ID = typeName;
            playerData.Upgrades.Add(upgradeProgress);
        }

        return upgradeProgress;
    }

    private float CalculateBonusEffect(IUpgradeEffect effect, int upgradeLevel)
    {
        return effect.UpgradeConfig.BaseEffect + effect.UpgradeConfig.EffectMultiplier * upgradeLevel;
    }

    private float CalculatePrice(IUpgradeEffect effect, int upgradeLevel)
    {
        return effect.UpgradeConfig.BasePrice + (upgradeLevel * effect.UpgradeConfig.PriceMultiplier);
    }
}
