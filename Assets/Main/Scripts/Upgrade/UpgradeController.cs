using Main.Scripts.Views;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class UpgradeController : IInitializable
{
    private readonly UpgradeShopView upgradeShopView;
    private readonly List<IUpgradeEffect> effects;
    private readonly PlayerData playerData;
    private readonly AudioPlayer audioPlayer;
    private readonly SoundConfig soundConfig;
    private readonly List<UpgradeProgress> savedProgress;

    private readonly Dictionary<string, IUpgradeEffect> effectMap = new();
    private Dictionary<string, UpgradeView> upgradeViews;

    public UpgradeController(UpgradeShopView upgradeShopView, List<IUpgradeEffect> effects, PlayerData playerData, AudioPlayer audioPlayer, SoundConfig soundConfig)
    {
        this.upgradeShopView = upgradeShopView;
        this.effects = effects;
        this.playerData = playerData;
        this.audioPlayer = audioPlayer;
        this.soundConfig = soundConfig;
    }

    public void Initialize()
    {
        upgradeViews = new Dictionary<string, UpgradeView>();

        InitializeUpgrades();
        ApplyActiveUpgrades();

        for (int i = 0; i < effects.Count; i++)
        {
            IUpgradeEffect effect = effects[i];
            var view = GameObject.Instantiate(upgradeShopView.UpgradeViewPrefab, upgradeShopView.ContentContainer);
            view.GetComponent<RectTransform>().localPosition = new Vector3(0, -i * upgradeShopView.OffsetBetweenUpgradeView.y, 0);
           
           view.Buy.onClick.AddListener(() =>
           {
               Buy(effect.GetType().Name);
               audioPlayer.PlaySFX(soundConfig.BuyUpgradeSound, soundConfig.BuyUpgradeVolume);
           });

            UpgradeProgress upgradeProgress = GetUpgrade(effect.UpgradeConfig.Type.ToString());
            int upgradeLevel = upgradeProgress != null ? upgradeProgress.Level : 0;

            RedrawUpgradeView(effect, view, upgradeLevel);

            upgradeViews.Add(effect.UpgradeConfig.Type.ToString(), view);
        }
    }

    private void RedrawUpgradeView(IUpgradeEffect effect, UpgradeView view, int upgradeLevel)
    {
        float currentEffect = effect.UpgradeConfig.BaseEffect + effect.UpgradeConfig.EffectMultiplier * (upgradeLevel + 1);

        string description = "";

        if (upgradeLevel == 0)
        {
            description =
                effect.UpgradeConfig.DescriptionPrefix +
                $" {currentEffect:0.00} " +
                effect.UpgradeConfig.DescriptionSuffix;
        }
        else
        {
            float nextEffect = effect.UpgradeConfig.BaseEffect + effect.UpgradeConfig.EffectMultiplier * (upgradeLevel + 2);

            description =
                effect.UpgradeConfig.DescriptionPrefix +
                $" <b>{currentEffect:0.00}</b>" +
                $" <b><color=green>(+{nextEffect:0.00})</color></b> " +
                effect.UpgradeConfig.DescriptionSuffix;
        }

        view.Description.text = description;
        view.Level.gameObject.SetActive(upgradeLevel > 0);
        view.Level.text = "Level:" + upgradeLevel;
        view.Price.text = (effect.UpgradeConfig.BasePrice + (upgradeLevel * effect.UpgradeConfig.PriceMultiplier)).ToString() + "$";
    }

    private void InitializeUpgrades()
    {
        foreach (var effect in effects)
        {
            string typeName = effect.GetType().Name;
            effectMap[typeName] = effect;
        }
    }

    private void ApplyActiveUpgrades()
    {
        if(playerData.Upgrades == null)
        {
            playerData.Upgrades = new List<UpgradeProgress>();
        }

        foreach (var upgrade in playerData.Upgrades)
        {
            IUpgradeEffect effect = effectMap[upgrade.ID];
            effect.Apply(upgrade.Level);
        }
    }

    public void Buy(string upgradName)
    {
        string typeName = upgradName;

        var upgrade = GetUpgrade(typeName);

        if (upgrade == null)
        {
            upgrade = new UpgradeProgress { ID = typeName, Level = 0 };
            playerData.Upgrades.Add(upgrade);
        }

        upgrade.Level++;

        effectMap[typeName].Apply(upgrade.Level);

        RedrawUpgradeView(effectMap[typeName], upgradeViews[typeName], upgrade.Level);
    }

    private UpgradeProgress GetUpgrade(string typeName)
    {
        return playerData.Upgrades.FirstOrDefault(upgrade => upgrade.ID.Equals(typeName));
    }
}

public class AddClickDamageEffect : IUpgradeEffect
{
    private readonly GameData gameData;

    public UpgradeConfig UpgradeConfig {get; private set;}

    public AddClickDamageEffect(UpgradeConfig upgradeConfig, GameData gameData)
    {
        UpgradeConfig = upgradeConfig;
        this.gameData = gameData;
    }

    public void Apply(int level)
    {
        if (level > 1)
        {
            gameData.DamagePerClick -= UpgradeConfig.BaseEffect + (UpgradeConfig.BaseEffect * (UpgradeConfig.EffectMultiplier * (level - 1)));
        }

        gameData.DamagePerClick += UpgradeConfig.BaseEffect + (UpgradeConfig.BaseEffect * (UpgradeConfig.EffectMultiplier * level));
    }
}

public class AddDamagePerSecondTiear1Effect : IUpgradeEffect
{
    public UpgradeConfig UpgradeConfig { get; private set; }
    private readonly GameData gameData;

    public AddDamagePerSecondTiear1Effect(UpgradeConfig upgradeConfig, GameData gameData)
    {
        UpgradeConfig = upgradeConfig;
        this.gameData = gameData;
    }

    public void Apply(int level)
    {
        if(level > 1)
        {
            gameData.DamagePerSecond -= UpgradeConfig.BaseEffect + (UpgradeConfig.BaseEffect * (UpgradeConfig.EffectMultiplier * (level - 1)));
        }

        gameData.DamagePerSecond += UpgradeConfig.BaseEffect + (UpgradeConfig.BaseEffect * (UpgradeConfig.EffectMultiplier * level));
    }
}
