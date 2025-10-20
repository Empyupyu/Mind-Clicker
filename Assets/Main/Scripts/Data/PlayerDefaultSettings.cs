using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDefaultSettings", menuName = "Create Game Datas/PlayerDefaultSettings")]
public class PlayerDefaultSettings : ScriptableObject
{
    [field: SerializeField] public float DamagePerClick { get; private set; } = 1f;
    [field: SerializeField] public float DamagePerSecond { get; private set; } = 0f;
    [field: SerializeField] public float Cash { get; private set; } = 0f;

    [field: SerializeField] public int CurrentLevel { get; private set; } = 1;

    [field: SerializeField] public List<UpgradeProgress> Upgrades { get; private set; }
}
