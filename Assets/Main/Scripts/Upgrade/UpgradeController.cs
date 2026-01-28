using Main.Scripts.Views;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class UpgradeController : IInitializable
{
    private readonly UpgradeService upgradeService;
    private readonly IUpgradeViewFactory upgradeViewFactory;
    private readonly UpgradeShopView upgradeShopView;
    private readonly UpgradeSettings upgradeSettings;
    private List<Upgrade> upgrades;
    private List<UpgradeStateView> views = new();
    private int firstVisibleIndex;
    public UpgradeController(
    UpgradeService upgradeService,
    IUpgradeViewFactory upgradeViewFactory,
    UpgradeShopView upgradeShopView,
    UpgradeSettings upgradeSettings)
    {
        this.upgradeService = upgradeService;
        this.upgradeViewFactory = upgradeViewFactory;
        this.upgradeShopView = upgradeShopView;
        this.upgradeSettings = upgradeSettings;
    }

    public void Initialize()
    {
        upgrades = upgradeService.EffectsData.Values.ToList();
        upgradeService.OnUpgrade += UnlockNextUpgrade;

        upgradeShopView.ContentContainer.sizeDelta = new Vector2(upgradeShopView.ContentContainer.sizeDelta.x, upgrades.Count * upgradeSettings.ItemHeight);

        upgradeShopView.ScrollRect.onValueChanged.AddListener(_ => OnScrollChanged());

        InitializeUpgrades();

        CreateViews();
    }

    private void InitializeUpgrades()
    {
        for (int i = 0; i < upgrades.Count; i++)
        {
            var upgrade = upgrades[i];

            if (i < GameConstants.InitialUnlockedCount || upgrade.UpgradeProgress.Level > 0)
            {
                upgrade.SetAvailable(true);
            }
        }
    }

    private void CreateViews()
    {
        if(views.Count == 0)
        {
            for (int i = 0; i < upgradeSettings.VisibleCount; i++)
            {
                var view = upgradeViewFactory.Create();
                views.Add(view);
            }
        }
     
        Redraw();
    }

    private void UnlockNextUpgrade(Upgrade upgrade)
    {
        int index = upgrades.IndexOf(upgrade);
        index++;

        if(index < upgrades.Count)
        {
            var nextUpgrade = upgrades[index];

            if(nextUpgrade.IsUnlock == false)
            {
                nextUpgrade.SetAvailable(true);
            }
        }

        Redraw();
    }

    private void OnScrollChanged()
    {
        int newFirstIndex = Mathf.Clamp(
            Mathf.FloorToInt(upgradeShopView.ContentContainer.anchoredPosition.y / upgradeSettings.ItemHeight),
            0,
            Mathf.Max(0, upgrades.Count - upgradeSettings.VisibleCount)
        );

        if (newFirstIndex != firstVisibleIndex)
        {
            firstVisibleIndex = newFirstIndex;
            Redraw();
        }
    }

    private void Redraw()
    {
        for (int i = 0; i < views.Count; i++)
        {
            int dataIndex = firstVisibleIndex + i;
            if (dataIndex >= upgrades.Count)
            {
                continue;
            }

            var view = (RectTransform)views[i].transform;
            view.anchoredPosition = new Vector2(0, -dataIndex * upgradeSettings.ItemHeight);

            BindView(views[i], upgrades[dataIndex]);
        }
    }

    private void BindView(UpgradeStateView view, Upgrade upgrade)
    {
        view.SetState(upgrade.IsUnlock ? UpgradeViewState.Unlocked : UpgradeViewState.Locked);

        if (upgrade.IsUnlock)
            view.SubscribeToBuy(() => Buy(upgrade.Config.Title));

        view.UpdateContent(
            GetDescription(upgrade),
            upgrade.UpgradeProgress.Level,
            upgrade.Price
        );

        view.SetIcon(upgrade.Config.Icon);
    }

    private string GetDescription(Upgrade upgrade)
    {
        float currentEffect = upgrade.CurrentLevelBonusEffect;
        float nextLevelEffect = upgrade.NextLevelBonusEffect;
        UpgradeConfig upgradeConfig = upgrade.Config;

        if (upgrade.UpgradeProgress.Level == 0)
            return $"{upgradeConfig.DescriptionPrefix} {currentEffect:0.00} {upgradeConfig.DescriptionSuffix}";
        else
            return $"{upgradeConfig.DescriptionPrefix} <b>{currentEffect:0.00}</b> <b><color=green>(+{nextLevelEffect:0.00})</color></b> {upgradeConfig.DescriptionSuffix}";
    }

    private void Buy(string upgradeType)
    {
        upgradeService.Buy(upgradeType);
    }
}
