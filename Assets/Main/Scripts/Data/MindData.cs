using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MindData", menuName = "Create Game Datas/Mind Data")]
public class MindData : ScriptableObject
{
    [field: SerializeField] public List<MindLevel> MindLevels { get; private set; }
    [field: SerializeField] public float BaseGainRate { get; private set; } = 1f;
    [field: SerializeField] public AudioClip UpgradeSound { get; private set; }
    [field: SerializeField] public float SoundVolume { get; private set; } = 1f;
}
