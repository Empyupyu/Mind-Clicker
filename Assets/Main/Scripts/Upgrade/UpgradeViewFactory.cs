using Main.Scripts.Views;
using UnityEngine;

public class UpgradeViewFactory : IUpgradeViewFactory
{
    private readonly UpgradeShopView shopView;

    public UpgradeViewFactory(UpgradeShopView shopView)
    {
        this.shopView = shopView;
    }

    public UpgradeStateView Create(UpgradeConfig config, int index, bool unlocked)
    {
        var view = GameObject.Instantiate(shopView.UpgradeStateView, shopView.ContentContainer);
        view.GetComponent<RectTransform>().localPosition = new Vector3(0, -index * shopView.OffsetBetweenUpgradeView.y, 0);

        view.SetState(unlocked ? UpgradeViewState.Unlocked : UpgradeViewState.Locked);
        view.SetIcon(config.Icon);

        return view;
    }

    public UpgradeStateView CreateComingSoon(int index)
    {
        var view = GameObject.Instantiate(shopView.UpgradeStateView, shopView.ContentContainer);
        view.GetComponent<RectTransform>().localPosition = new Vector3(0, -index * shopView.OffsetBetweenUpgradeView.y, 0);
        view.SetState(UpgradeViewState.ComingSoon);
        return view;
    }
}
