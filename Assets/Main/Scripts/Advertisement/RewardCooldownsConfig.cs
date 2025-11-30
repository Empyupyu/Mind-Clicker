using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "RewardCooldownsConfig", menuName = "Create Game Datas/RewardCooldownsConfig")]
public class RewardCooldownsConfig : ScriptableObject
{
    [field: SerializeField] public List<RewardCooldownConfig> Cooldowns { get; private set; }
}
