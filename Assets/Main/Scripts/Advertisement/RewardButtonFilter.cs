using System;
using UnityEngine;

[Serializable]
public class RewardButtonFilter
{
    [field: SerializeField] public AdvertisementRewardID AdvertisementRewardID { get; private set; }
    [field: SerializeField] public bool IsAllowed { get; private set; }
}