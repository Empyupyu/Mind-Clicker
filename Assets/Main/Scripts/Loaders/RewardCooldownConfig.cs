using System;
using UnityEngine;

[Serializable]
public class RewardCooldownConfig
{
    public AdvertisementRewardID RewardId;
    public float CooldownHours;
    public bool ResetNextDay;

    public TimeSpan CooldownDuration => TimeSpan.FromHours(CooldownHours);
}
