using System;
using System.Collections.Generic;
using UnityEngine;

public class RewardButtonsController
{
    private readonly AdvertisementService ads;
    private readonly RewardSettings settings;
    private readonly AdvertisementRewardCooldownService cooldown;

    private Dictionary<AdvertisementRewardID, AdvertisementRewardButton> buttons;

    public RewardButtonsController(
        AdvertisementService ads,
        RewardSettings settings,
        AdvertisementRewardCooldownService cooldown)
    {
        this.ads = ads;
        this.settings = settings;
        this.cooldown = cooldown;
    }

    public void Initialize(List<AdvertisementRewardButton> rewardButtons)
    {
        if(buttons == null)
            buttons = new Dictionary<AdvertisementRewardID, AdvertisementRewardButton>();

        foreach (var b in rewardButtons)
        {
            var filter = settings.RewardButtonsFilter.Find(f => f.AdvertisementRewardID == b.ID);

            if (filter == null || !filter.IsAllowed)
            {
                b.gameObject.SetActive(false);
                continue;
            }

            b.Button.onClick.AddListener(() => ads.ShowReward(b.ID.ToString()));

            if (buttons.ContainsKey(b.ID))
            {
                Debug.LogWarning("Duplicate Reward Button ID = " + b.ID);
                continue;
            }

            buttons.Add(b.ID, b);
        }

        ads.OnRewardCompleted += OnRewarded;
    }

    private void OnRewarded(string id)
    {
        if (!Enum.TryParse(id, out AdvertisementRewardID rewardID))
            return;

        if (!buttons.TryGetValue(rewardID, out var button))
            return;

        bool available = cooldown.IsAvailable(rewardID);
        button.SetInteractable(available);
    }
}
