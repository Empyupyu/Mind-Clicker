using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
public class InterstitialAdvertisementController : IGameModule, IDisposable
{
    public int Priority { get; }

    private readonly AdvertisementService advertisementService;
    private readonly PlayerDataRef playerDataRef;
    private readonly IEnumerable<IAdvertisementTrigger> triggers;
    private readonly AudioPlayer audioPlayer;

    public InterstitialAdvertisementController(
        AdvertisementService advertisementService,
        PlayerDataRef playerDataRef,
        IEnumerable<IAdvertisementTrigger> triggers,
        int priority,
        AudioPlayer audioPlayer)
    {
        this.advertisementService = advertisementService;
        this.playerDataRef = playerDataRef;
        this.triggers = triggers;
        Priority = priority;
        this.audioPlayer = audioPlayer;
    }

    public async UniTask InitializeAsync()
    {
        advertisementService.EnableBanner(!AdvertisementIsDisabled());
        advertisementService.OnInterstitialCompleted += AfterInterstitialShowing;

        InitTriggers();

        await UniTask.CompletedTask;
    }

    private void AfterInterstitialShowing()
    {
        audioPlayer.MuteSounds(false);
    }

    private void InitTriggers()
    {
        foreach (var trigger in triggers)
            trigger.Initialize(() => TryShowInterstitial());
    }

    private void TryShowInterstitial()
    {
        if (AdvertisementIsDisabled()) return;

        audioPlayer.MuteSounds(true);
        advertisementService.ShowInterstitial();
    }

    private bool AdvertisementIsDisabled()
    {
        return playerDataRef.Value.AdvertisementIsDisabled;
    }

    public void Dispose()
    {
        advertisementService.OnInterstitialCompleted -= AfterInterstitialShowing;
    }
}
