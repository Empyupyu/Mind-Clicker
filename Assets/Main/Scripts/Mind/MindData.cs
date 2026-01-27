using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MindData", menuName = "Create Game Datas/Mind Data")]
public class MindData : ScriptableObject
{
    [field: SerializeField] public List<MindLevel> MindLevels { get; private set; }
    [field: SerializeField] public float BaseGainRate { get; private set; } = 1f;
    [field: SerializeField] public AudioClip UpgradeSound { get; private set; }
    [field: SerializeField] public float SoundVolume { get; private set; } = 1f;
    [field: SerializeField] public float LevelUpProgressBarDuration { get; private set; } = 0.3f;
    [field: SerializeField] public float LevelUpProgressBarTargetValue { get; private set; } = 1f;
    [field: SerializeField] public float LevelUpProgressBarShakeDuration { get; private set; } = 0.3f;
    [field: SerializeField] public float LevelUpProgressBarShakeStrength { get; private set; } = 10f;
    [field: SerializeField] public int LevelUpProgressBarShakeVibrato { get; private set; } = 5;
    [field: SerializeField] public float LevelUpProgressBarShakeRandomness { get; private set; } = 15f;
}
