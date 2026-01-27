using Main.Scripts.Views;
using UnityEngine;

public class UpgradeViewFactory : IUpgradeViewFactory
{
    private readonly UpgradeShopView shopView;

    public UpgradeViewFactory(UpgradeShopView shopView)
    {
        this.shopView = shopView;
    }

    public UpgradeStateView Create()
    {
        return GameObject.Instantiate(shopView.UpgradeStateView, shopView.ContentContainer);
    }
}
