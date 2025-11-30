using System;
using System.Collections.Generic;
using System.Linq;

public class AdvertisementRewardCooldownService
{
    private readonly Dictionary<AdvertisementRewardID, DateTime> cooldowns = new();
    private readonly Dictionary<AdvertisementRewardID, RewardCooldownConfig> configs;

    public AdvertisementRewardCooldownService(RewardCooldownsConfig config)
    {
        this.configs = config.Cooldowns.ToDictionary(c => c.RewardId);
    }

    public bool IsAvailable(AdvertisementRewardID rewardId)
    {
        if (!cooldowns.TryGetValue(rewardId, out var until))
            return true;

        return DateTime.UtcNow >= until;
    }

    public void ApplyCooldown(AdvertisementRewardID rewardId)
    {
        if (!configs.TryGetValue(rewardId, out var config))
            return;

        if (config.ResetNextDay)
        {
            cooldowns[rewardId] = DateTime.UtcNow.Date.AddDays(1);
        }
        else
        {
            cooldowns[rewardId] = DateTime.UtcNow.Add(config.CooldownDuration);
        }
    }
}
