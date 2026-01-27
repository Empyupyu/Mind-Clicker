using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Action = System.Action;

public class TimerAdvertisementTrigger : IAdvertisementTrigger
{
    private readonly AddressableAssetLoader addressableAssetLoader;
    private readonly DiContainer diContainer;
    private readonly AdvertisementData advertisementData;
    private AdvertisementTimerView advertisementTimerViewInstance;
   
    public TimerAdvertisementTrigger(
        AddressableAssetLoader addressableAssetLoader,
        DiContainer diContainer,
        AdvertisementData advertisementData) 
    {
        this.addressableAssetLoader = addressableAssetLoader;
        this.diContainer = diContainer;
        this.advertisementData = advertisementData;
    }

    public void Initialize(Action showInterstitial)
    {
        Timer timer = new Timer();
        timer.OnFinished += () =>
        {
            ShowTimerBeforeAdvertisement(showInterstitial).Forget();
        };
        timer.StartTimer(advertisementData.InterstitialColdown).Forget();
    }

    private async UniTask ShowTimerBeforeAdvertisement(Action showInterstitial)
    {
        await InstantiateTimerView();

        advertisementTimerViewInstance.gameObject.SetActive(true);
        advertisementTimerViewInstance.Popup.Show();

        Timer timer = new Timer();
        timer.OnTick += (x) => advertisementTimerViewInstance.Redraw(((int)x).ToString());
        timer.StartTimer(advertisementData.PreInterstitialViewDuration).Forget();

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
