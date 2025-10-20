using Main.Scripts.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class UpgradeController : IInitializable, IDisposable
{
    private readonly Upgrade upgrade;
    private readonly UpgradeShopView upgradeShopView;
    private readonly AudioPlayer audioPlayer;
    private readonly SoundConfig soundConfig;

    private Dictionary<string, UpgradeView> upgradeViews;

    public UpgradeController(Upgrade upgrade, UpgradeShopView upgradeShopView, AudioPlayer audioPlayer, SoundConfig soundConfig)
    {
        this.upgrade = upgrade;
        this.upgradeShopView = upgradeShopView;
        this.audioPlayer = audioPlayer;
        this.soundConfig = soundConfig;
    }

    public void Initialize()
    {
        upgradeViews = new Dictionary<string, UpgradeView>();
        upgrade.OnUpgrade += OnUpgrade;

        var upgrades = upgrade.EffectsData.Values.ToList();

        for (int i = 0; i < upgrades.Count; i++)
        {
            IUpgradeEffect effect = upgrades[i].Effect;
            UpgradeView view = GameObject.Instantiate(upgradeShopView.UpgradeViewPrefab, upgradeShopView.ContentContainer);
            view.GetComponent<RectTransform>().localPosition = new Vector3(0, -i * upgradeShopView.OffsetBetweenUpgradeView.y, 0);
           
           view.Buy.onClick.AddListener(() =>
           {
               Buy(effect.GetType().Name);           
           });

            upgradeViews.Add(effect.UpgradeConfig.Type.ToString(), view);
            RedrawUpgradeView(upgrades[i]);
        }
    }

    private void OnUpgrade(UpgradeData data)
    {
        RedrawUpgradeView(data);
        audioPlayer.PlaySFX(soundConfig.BuyUpgradeSound, soundConfig.BuyUpgradeVolume);
    }

    private void RedrawUpgradeView(UpgradeData upgradeData)
    {
        float currentEffect = upgradeData.CurrentLevelBonusEffect;
        float nextLevelEffect = upgradeData.NextLevelBonusEffect;
        float price = upgradeData.Price;
        int level = upgradeData.Level;

        UpgradeConfig upgradeConfig = upgradeData.Effect.UpgradeConfig;
        UpgradeView view = upgradeViews[upgradeConfig.Type.ToString()];

        string description = "";

        if (upgradeData.Level == 0)
        {
            description =
                upgradeConfig.DescriptionPrefix +
                $" {currentEffect:0.00} " +
                upgradeConfig.DescriptionSuffix;
        }
        else
        {
            float nextEffect = nextLevelEffect;

            description =
                upgradeConfig.DescriptionPrefix +
                $" <b>{currentEffect:0.00}</b>" +
                $" <b><color=green>(+{nextEffect:0.00})</color></b> " +
                upgradeConfig.DescriptionSuffix;
        }

        view.Description.text = description;
        view.Level.gameObject.SetActive(level > 0);
        view.Level.text = "Level:" + level;
        view.Price.text = price.ToString() + "$";
    }

    public void Buy(string upgradeType)
    {
        upgrade.Buy(upgradeType);
    }

    public void Dispose()
    {
        foreach (var view in upgradeViews.Values)
        {
            view.Buy.onClick.RemoveAllListeners();
        }
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
