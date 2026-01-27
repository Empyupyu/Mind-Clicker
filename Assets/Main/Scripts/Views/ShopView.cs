using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopView : MonoBehaviour
{
    [field: SerializeField] public List<AdvertisementRewardButton> RewardButtons { get; private set; }
    [field: SerializeField] public Button CloseButton { get; private set; }
}
