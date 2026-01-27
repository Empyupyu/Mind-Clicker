using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RewardSettings", menuName = "Create Game Datas/RewardSettings")]
public class RewardSettings : ScriptableObject
{
    //[field: SerializeField] public RewardCooldownsConfig RewardCooldownsConfig { get; private set; }
    [field: SerializeField] public List<RewardButtonFilter> RewardButtonsFilter { get; private set; }
}
