using System;
using System.Collections.Generic;
using System.Linq;

public class AdvertisementRewardRegistry
{
    private readonly Dictionary<string, IRewardHandler> handlers;

    public AdvertisementRewardRegistry(IEnumerable<IRewardHandler> rewardHandlers)
    {
        handlers = rewardHandlers.ToDictionary(h => h.GetType().Name);
    }

    public IRewardHandler Get(string id)
    {
        if (!handlers.TryGetValue(id, out var handler))
            throw new Exception($"Reward handler not found for id: {id}");
        return handler;
    }
}
