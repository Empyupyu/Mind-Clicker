using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Action = System.Action;

public class TimerAdvertisementTrigger : IAdvertisementTrigger
{
    private readonly AddressableAssetLoader addressableAssetLoader;
    private readonly DiContainer diContainer;
    private AdvertisementTimerView advertisementTimerViewInstance;
   
    public TimerAdvertisementTrigger(AddressableAssetLoader addressableAssetLoader, DiContainer diContainer) 
    {
        this.addressableAssetLoader = addressableAssetLoader;
        this.diContainer = diContainer;
    }

    public void Initialize(Action showInterstitial)
    {
        Timer timer = new Timer();
        timer.OnFinished += () =>
        {
            ShowTimerBeforeAdvertisement(showInterstitial).Forget();
        };
        timer.StartTimer(90).Forget();
    }

    private async UniTask ShowTimerBeforeAdvertisement(Action showInterstitial)
    {
        await InstantiateTimerView();

        advertisementTimerViewInstance.gameObject.SetActive(true);
        advertisementTimerViewInstance.Popup.Show();

        Timer timer = new Timer();
        timer.OnTick += (x) => advertisementTimerViewInstance.Redraw(((int)x).ToString());
        timer.StartTimer(4).Forget();

        timer.OnFinished += () =>
        {
            advertisementTimerViewInstance.Popup.Hide();
            showInterstitial.Invoke();
            Initialize(showInterstitial);
        };
    }

    private async UniTask InstantiateTimerView()
    {
        if (advertisementTimerViewInstance == null)
        {
            var asset = await addressableAssetLoader.LoadAsset<GameObject>(GameConstants.TimerViewKey);
            advertisementTimerViewInstance = diContainer.InstantiatePrefab(asset.gameObject).GetComponent<AdvertisementTimerView>();
        }
    }
}

public static class GameConstants
{
    public const string LevelKey = "Game";
    public const string VolumeParam = "MasterVolume";
    public const string SaveKey = "PlayerDataSaveKey";
    public const string TimerViewKey = "Advertisement Timer View";
    public const int AuthorizationTimeout = 90;
    public const int FakeLoadingDelay = 1000;
    public const int InitialUnlockedCount = 1;
}