using System.Collections.Generic;
using System.Linq;
using Zenject;

//TODO
public class UpgradeController : IInitializable
{
    private readonly Upgrade upgrade;
    private readonly IUpgradeViewFactory upgradeViewFactory;
    private Dictionary<string, UpgradeStateView> upgradeViews;
    private const int InitialUnlockedCount = 1;

    public UpgradeController(Upgrade upgrade, IUpgradeViewFactory upgradeViewFactory)
    {
        this.upgrade = upgrade;
        this.upgradeViewFactory = upgradeViewFactory;
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

            bool isUnlocked = i < InitialUnlockedCount || upgradeData.UpgradeProgress.Level > 0;
            var view = upgradeViewFactory.Create(effect, i, isUnlocked);

            if (isUnlocked)
            {
                InitializeBuyView(effect, view);
            }

            upgradeViews.Add(effect.UpgradeConfig.Type.ToString(), view);
            RedrawUpgradeView(upgradeData);
        }

        upgradeViewFactory.CreateComingSoon(upgrades.Count);
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
