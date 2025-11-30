using Cysharp.Threading.Tasks;
using System;
using Zenject;

public class AdvertisementRewardService : IGameModule
{
    public int Priority { get; }

    private readonly IAdvertisementProvider advertisementProvider;
    private readonly AdvertisementRewardRegistry rewardRegistry;
    private readonly AdvertisementRewardCooldownService cooldownService;
    private readonly SignalBus signalBus;

    public AdvertisementRewardService(
        IAdvertisementProvider provider,
        AdvertisementRewardRegistry rewardRegistry,
        AdvertisementRewardCooldownService cooldownService,
        SignalBus signalBus,
        int priority)
    {
        advertisementProvider = provider;
        this.rewardRegistry = rewardRegistry;
        this.cooldownService = cooldownService;
        this.signalBus = signalBus;
        Priority = priority;
    }

    public async UniTask InitializeAsync()
    {
        advertisementProvider.OnRewardCompleted += OnRewardCompleted;
        await UniTask.CompletedTask;
    }

    private void OnRewardCompleted(string id)
    {
        if (Enum.TryParse(id, out AdvertisementRewardID rewardId))
        {
            rewardRegistry.Get(id).Apply();
            cooldownService.ApplyCooldown(rewardId);

            bool available = cooldownService.IsAvailable(rewardId);
            signalBus.Fire(new RewardCooldownUpdatedSignal(rewardId, available));
        }
    }
}
