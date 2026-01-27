using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class ShopController : IInitializable
{
    private readonly UIWindowsView uIWindowsView;
    private readonly RewardButtonsController rewardButtonsController;
    private readonly AddressableAssetLoader addressableAssetLoader;
    private ShopView shopViewInstance;

    public ShopController(UIWindowsView uIWindowsView,
        RewardButtonsController rewardButtonsController,
        AddressableAssetLoader addressableAssetLoader)
    {
        this.uIWindowsView = uIWindowsView;
        this.rewardButtonsController = rewardButtonsController;
        this.addressableAssetLoader = addressableAssetLoader;
    }

    public void Initialize()
    {
        uIWindowsView.ShopButton.onClick.AddListener(() => OpenWindow().Forget());
    }

    private async UniTaskVoid OpenWindow()
    {
        if(shopViewInstance != null)
        {
            addressableAssetLoader.Unload("ShopView").Forget();
        }

        var view = await addressableAssetLoader.LoadPrefab("ShopView");
        shopViewInstance = view.GetComponent<ShopView>();

        shopViewInstance.CloseButton.onClick.AddListener(() => 
        {
            CloseWindow().Forget();
        });
        rewardButtonsController.Initialize(shopViewInstance.RewardButtons);
    }

    private async UniTaskVoid CloseWindow()
    {
        if (shopViewInstance != null)
        {
           await addressableAssetLoader.Unload("ShopView");
        }

        shopViewInstance = null;
    }
}
