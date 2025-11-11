using Cysharp.Threading.Tasks;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;
using Action = System.Action;

public class SaveLoadService
{
    private readonly IPlayerDataProvider provider;
    private const string SAVE_KEY = "SavePlayerDataKey";

    public SaveLoadService(IPlayerDataProvider provider)
    {
        this.provider = provider;
    }

    public UniTask<PlayerData> Load(string key) => provider.Load(key);
    public UniTask Save(PlayerData data) => provider.Save(SAVE_KEY, data);
}

public class AdvertisementService
{
    private readonly IAdvertisementProvider advertisementProvider;

    public AdvertisementService(IAdvertisementProvider provider)
    {
        advertisementProvider = provider;
    }

    public void EnableBanner(bool isActive)
    {
        advertisementProvider.EnableBanner(isActive); 
    }

    public void ShowInterstitial()
    {
        advertisementProvider.ShowInterstitial();
    }

    public void ShowReward(string id)
    {
        advertisementProvider.ShowReward(id);
    }
}

public interface IAdvertisementTrigger
{
    void Initialize(Action showInterstitial);
}

public class TimerAdvertisementTrigger : IAdvertisementTrigger
{
    private readonly TextMeshProUGUI text;

    public TimerAdvertisementTrigger(TextMeshProUGUI text) 
    {
        this.text = text;
    }

    public void Initialize(Action showInterstitial)
    {
        Timer timer = new Timer();
        timer.OnFinished += () =>
        {
            Timer timer = new Timer();
            timer.OnTick += (x) => text.text = ((int)x).ToString();
            timer.StartTimer(3).Forget();

            timer.OnFinished += showInterstitial;
        };
        timer.StartTimer(90f).Forget();
    }
}

[RequireComponent(typeof(Button))]
public class AdvertisementRewardButton : MonoBehaviour
{
    [SerializeField] private AdvertisementRewardID id;

    [Inject] private SignalBus signalBus;

    public void SetID(AdvertisementRewardID id)
    {
        this.id = id;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
            signalBus.Fire(new AdvertisementRewardButtonClickedSignal(id)));
    }
}

public enum AdvertisementRewardID
{
    DisabledAdvertisement,
    HardCurrency
}

public class AdvertisementRewardButtonClickedSignal
{
    public AdvertisementRewardID RewardId { get; }
    public AdvertisementRewardButtonClickedSignal(AdvertisementRewardID rewardId) => RewardId = rewardId;
}

public class AdvertisementController
{
    private readonly AdvertisementService advertisementService;
    private readonly ShopView shopView;
    private readonly PlayerDataRef playerDataRef;
    private readonly SignalBus signalBus;
    private readonly IEnumerable<IAdvertisementTrigger> triggers;
    private readonly TextMeshProUGUI TEST;

    public AdvertisementController(AdvertisementService advertisementService, ShopView shopView, PlayerDataRef playerDataRef, SignalBus signalBus, IEnumerable<IAdvertisementTrigger> triggers)
    {
        this.advertisementService = advertisementService;
        this.shopView = shopView;
        this.playerDataRef = playerDataRef;
        this.signalBus = signalBus;
        this.triggers = triggers;

        advertisementService.EnableBanner(!AdvertisementIsDisabled());

        InitTriggers(triggers);
    }

    private void InitTriggers(IEnumerable<IAdvertisementTrigger> triggers)
    {
        signalBus.Subscribe<AdvertisementRewardButtonClickedSignal>(OnBuyButtonClicked);

        foreach (var trigger in triggers)
            trigger.Initialize(() => TryShowInterstitial());
    }

    private void OnBuyButtonClicked(AdvertisementRewardButtonClickedSignal signal)
    {
        advertisementService.ShowReward(signal.RewardId.ToString());
    }

    private void TryShowInterstitial()
    {
        if (AdvertisementIsDisabled()) return;

        advertisementService.ShowInterstitial();
    }

    private bool AdvertisementIsDisabled()
    {
        return playerDataRef.Value.AdvertisementIsDisabled;
    }
}

public class ShopView : MonoBehaviour
{
    [field: SerializeField] public Button BuyButtonPrefab { get; private set; }
}

public class AdvertisementRewardService
{
    private readonly IAdvertisementProvider advertisementProvider;
    private readonly List<IRewardHandler> rewardHandlers;

    public AdvertisementRewardService(IAdvertisementProvider provider, List<IRewardHandler> rewardHandlers)
    {
        advertisementProvider = provider;
        this.rewardHandlers = rewardHandlers;

        advertisementProvider.OnRewardCompleted += OnRewardCompleted;
    }

    private void OnRewardCompleted(string id)
    {
        rewardHandlers[0].Apply();
    }
}

public interface IRewardHandler
{
    public void Apply();
}

public class HardCurrencyReward : IRewardHandler
{
    private readonly int ammount;
    private readonly PlayerDataRef playerDataRef;

    public HardCurrencyReward(int ammount, PlayerDataRef playerDataRef)
    {
        this.ammount = ammount;
        this.playerDataRef = playerDataRef;
    }

    public void Apply()
    {
        playerDataRef.Value.HardCurrency += ammount;
    }
}

public class DisabledAdvertisementReward : IRewardHandler
{
    private readonly PlayerDataRef playerDataRef;

    public DisabledAdvertisementReward(PlayerDataRef playerDataRef)
    {
        this.playerDataRef = playerDataRef;
    }

    public void Apply()
    {
        playerDataRef.Value.AdvertisementIsDisabled = true;
    }
}

public class YandexAdvertisementProvider : AdvertisementProviderBase
{
    public override void EnableBanner(bool enableBanner)
    {
        YG2.StickyAdActivity(enableBanner);
    }

    public override void ShowInterstitial()
    {
        YG2.InterstitialAdvShow();
    }

    public override void ShowReward(string id)
    {
        YG2.RewardedAdvShow(id);
    }

    protected override void SubscribeEvents()
    {
        YG2.onCloseInterAdv += HandleInterstitialClosed;
        YG2.onRewardAdv += HandleRewarded;
    }

    protected override void UnsubscribeEvents()
    {
        YG2.onCloseInterAdv -= HandleInterstitialClosed;
        YG2.onRewardAdv -= HandleRewarded;
    }

    private void HandleInterstitialClosed()
    {
        RaiseInterstitialCompleted();
    }

    private void HandleRewarded(string id)
    {
        RaiseRewardCompleted(id);
    }
}
