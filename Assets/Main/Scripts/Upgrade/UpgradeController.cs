using Main.Scripts.Views;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class UpgradeController : IInitializable
{
    private readonly Upgrade upgrade;
    private readonly UpgradeShopView upgradeShopView;
    private Dictionary<string, UpgradeStateView> upgradeViews;
    private const int InitialUnlockedCount = 1;

    public UpgradeController(Upgrade upgrade, UpgradeShopView upgradeShopView)
    {
        this.upgrade = upgrade;
        this.upgradeShopView = upgradeShopView;
    }

    public void Initialize()
    {
        upgrade.OnUpgrade += OnUpgrade;

        CreateUpgradeViews();
    }

    private void CreateUpgradeViews()
    {
        upgradeViews = new Dictionary<string, UpgradeStateView>();
        var upgrades = upgrade.EffectsData.Values.ToList();

        for (int i = 0; i < upgrades.Count; i++)
        {
            var upgradeData = upgrades[i];
            var effect = upgradeData.Effect;

            var view = GameObject.Instantiate(upgradeShopView.UpgradeStateView, upgradeShopView.ContentContainer);
            view.GetComponent<RectTransform>().localPosition = new Vector3(0, -i * upgradeShopView.OffsetBetweenUpgradeView.y, 0);

            bool isUnlocked = i < InitialUnlockedCount;
            var state = isUnlocked ? UpgradeViewState.Unlocked : UpgradeViewState.Locked;

            view.SetState(state);

            if (isUnlocked)
            {
                InitializeBuyView(effect, view);
            }

            upgradeViews.Add(effect.UpgradeConfig.Type.ToString(), view);
            RedrawUpgradeView(upgradeData);
        }

        // ComingSoon
        var comingSoonView = GameObject.Instantiate(upgradeShopView.UpgradeStateView, upgradeShopView.ContentContainer);
        comingSoonView.GetComponent<RectTransform>().localPosition = new Vector3(0, -upgrades.Count * upgradeShopView.OffsetBetweenUpgradeView.y, 0);
        comingSoonView.SetState(UpgradeViewState.ComingSoon);
    }

    private void InitializeBuyView(IUpgradeEffect effect, UpgradeStateView view)
    {
        view.SubscribeToBuy(() => Buy(effect.GetType().Name));
        view.SetIcon(effect.UpgradeConfig.Icon);
    }

    private void OnUpgrade(UpgradeData data)
    {
        RedrawUpgradeView(data);

        var upgrades = upgrade.EffectsData.Values.ToList();
        int index = upgrades.FindIndex(u => u.Effect.UpgradeConfig.Type == data.Effect.UpgradeConfig.Type);

        int nextIndex = index + 1;
        if (nextIndex < upgrades.Count)
        {
            var nextData = upgrades[nextIndex];
            var nextView = upgradeViews[nextData.Effect.UpgradeConfig.Type.ToString()];

            if (nextView != null)
            {
                nextView.SetState(UpgradeViewState.Unlocked);
                InitializeBuyView(nextData.Effect, nextView);
                RedrawUpgradeView(nextData);
            }
        }
    }

    private void RedrawUpgradeView(UpgradeData upgradeData)
    {
        var view = upgradeViews[upgradeData.Effect.UpgradeConfig.Type.ToString()];
        var stateView = view;

        if (stateView == null)
            return;

        if (stateView.CurrentState != UpgradeViewState.Unlocked)
            return;

        float price = upgradeData.Price;
        int level = upgradeData.UpgradeProgress.Level;
        string desc = GetDescription(upgradeData);

        stateView.UpdateContent(desc, level, price);
    }

    private string GetDescription(UpgradeData upgradeData)
    {
        float currentEffect = upgradeData.CurrentLevelBonusEffect;
        float nextLevelEffect = upgradeData.NextLevelBonusEffect;
        UpgradeConfig upgradeConfig = upgradeData.Effect.UpgradeConfig;

        string description;

        if (upgradeData.UpgradeProgress.Level == 0)
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

        return description;
    }

    private void Buy(string upgradeType)
    {
        upgrade.Buy(upgradeType);
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
