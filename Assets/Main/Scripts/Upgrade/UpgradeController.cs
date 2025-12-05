using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class UpgradeController : IInitializable
{
    private readonly UpgradeService upgradeService;
    private readonly IUpgradeViewFactory upgradeViewFactory;
    private Dictionary<string, UpgradeStateView> upgradeViews;

    public UpgradeController(UpgradeService upgradeService, IUpgradeViewFactory upgradeViewFactory)
    {
        this.upgradeService = upgradeService;
        this.upgradeViewFactory = upgradeViewFactory;
    }

    public void Initialize()
    {
        upgradeService.OnUpgrade += OnUpgrade;

        CreateUpgradeViews();
    }

    public void RedrawViews()
    {
        foreach (var view in upgradeViews.Values)
        {
            GameObject.Destroy(view.gameObject);
        }

        CreateUpgradeViews();
    }

    private void CreateUpgradeViews()
    {
        upgradeViews = new Dictionary<string, UpgradeStateView>();
        var upgrades = upgradeService.EffectsData.Values.ToList();

        for (int i = 0; i < upgrades.Count; i++)
        {
            var upgrade = upgrades[i];
            var config = upgrade.Config;

            bool isUnlocked = i < GameConstants.InitialUnlockedCount || upgrade.UpgradeProgress.Level > 0;
            var view = upgradeViewFactory.Create(config, i, isUnlocked);

            if (isUnlocked)
            {
                InitializeBuyView(config, view);
            }

            upgradeViews.Add(config.Title, view);
            RedrawUpgradeView(upgrade);
        }

        upgradeViewFactory.CreateComingSoon(upgrades.Count);
    }

    private void InitializeBuyView(UpgradeConfig config, UpgradeStateView view)
    {
        view.SubscribeToBuy(() => Buy(config.Title));
        view.SetIcon(config.Icon);
    }

    private void OnUpgrade(Upgrade data)
    {
        RedrawUpgradeView(data);
        SetButtonState(data);
    }

    private void SetButtonState(Upgrade upgrade)
    {
        var upgrades = upgradeService.EffectsData.Values.ToList();
        int index = upgrades.FindIndex(u => u.Config.Title == upgrade.Config.Title);

        int nextIndex = index + 1;
        if (nextIndex < upgrades.Count)
        {
            var nextUpgrade = upgrades[nextIndex];
            var nextView = upgradeViews[nextUpgrade.Config.Title];

            if (nextView != null)
            {
                nextView.SetState(UpgradeViewState.Unlocked);
                InitializeBuyView(nextUpgrade.Config, nextView);
                RedrawUpgradeView(nextUpgrade);
            }
        }
    }

    private void RedrawUpgradeView(Upgrade upgrade)
    {
        var view = upgradeViews[upgrade.Config.Title];
        var stateView = view;

        if (stateView == null)
            return;

        if (stateView.CurrentState != UpgradeViewState.Unlocked)
            return;

        float price = upgrade.Price;
        int level = upgrade.UpgradeProgress.Level;
        string desc = GetDescription(upgrade);

        stateView.UpdateContent(desc, level, price);
    }

    private string GetDescription(Upgrade upgrade)
    {
        float currentEffect = upgrade.CurrentLevelBonusEffect;
        float nextLevelEffect = upgrade.NextLevelBonusEffect;
        UpgradeConfig upgradeConfig = upgrade.Config;

        string description;

        if (upgrade.UpgradeProgress.Level == 0)
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
        upgradeService.Buy(upgradeType);
    }
}
