using Cysharp.Threading.Tasks;
using System;

public class AdvertisementRewardService : IGameModule
{
    public int Priority { get; }

    private readonly IAdvertisementProvider advertisementProvider;
    private readonly AdvertisementRewardRegistry rewardRegistry;
    private readonly AdvertisementRewardCooldownService cooldownService;

    public AdvertisementRewardService(
        IAdvertisementProvider provider,
        AdvertisementRewardRegistry rewardRegistry,
        AdvertisementRewardCooldownService cooldownService,
        int priority)
    {
        advertisementProvider = provider;
        this.rewardRegistry = rewardRegistry;
        this.cooldownService = cooldownService;
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
        }
    }
}
