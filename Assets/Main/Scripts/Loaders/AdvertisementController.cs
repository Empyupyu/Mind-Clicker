using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Zenject;

public class AdvertisementController : IGameModule, IDisposable
{
    public int Priority { get; }

    private readonly AdvertisementService advertisementService;
    private readonly PlayerDataRef playerDataRef;
    private readonly SignalBus signalBus;
    private readonly IEnumerable<IAdvertisementTrigger> triggers;
    private readonly AdvertisementRewardCooldownService cooldownService;

    public AdvertisementController(
        AdvertisementService advertisementService,
        PlayerDataRef playerDataRef,
        SignalBus signalBus,
        IEnumerable<IAdvertisementTrigger> triggers,
        AdvertisementRewardCooldownService cooldownService,
        int priority)
    {
        this.advertisementService = advertisementService;
        this.playerDataRef = playerDataRef;
        this.signalBus = signalBus;
        this.triggers = triggers;
        this.cooldownService = cooldownService;
        Priority = priority;
    }

    public async UniTask InitializeAsync()
    {
        advertisementService.EnableBanner(!AdvertisementIsDisabled());
        advertisementService.OnInterstitialCompleted += AfterInterstitialShowing;

        signalBus.Subscribe<AdvertisementRewardButtonClickedSignal>(OnBuyButtonClicked);
        signalBus.Subscribe<RewardButtonInitializedSignal>(OnButtonInitialized);

        InitTriggers();

        await UniTask.CompletedTask;
    }

    private void AfterInterstitialShowing()
    {
        signalBus.Fire(new MuteSoundsSignal(false));
    }

    public void Dispose()
    {
        advertisementService.OnInterstitialCompleted -= AfterInterstitialShowing;

        signalBus.Unsubscribe<AdvertisementRewardButtonClickedSignal>(OnBuyButtonClicked);
        signalBus.Unsubscribe<RewardButtonInitializedSignal>(OnButtonInitialized);
    }

    private void InitTriggers()
    {
        foreach (var trigger in triggers)
            trigger.Initialize(() => TryShowInterstitial());
    }

    private void OnButtonInitialized(RewardButtonInitializedSignal signal)
    {
        bool available = cooldownService.IsAvailable(signal.RewardId);
        signal.Button.SetInteractable(available);
    }

    private void OnBuyButtonClicked(AdvertisementRewardButtonClickedSignal signal)
    {
        advertisementService.ShowReward(signal.RewardId.ToString());
    }

    private void TryShowInterstitial()
    {
        if (AdvertisementIsDisabled()) return;

        signalBus.Fire(new MuteSoundsSignal(true));
        advertisementService.ShowInterstitial();
    }

    private bool AdvertisementIsDisabled()
    {
        return playerDataRef.Value.AdvertisementIsDisabled;
    }
}
