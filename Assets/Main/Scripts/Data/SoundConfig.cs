using UnityEngine;

[CreateAssetMenu(fileName = "SoundConfig", menuName = "Create Game Datas/SoundConfig")]
public class SoundConfig : ScriptableObject
{
    [field: SerializeField] public AudioClip ClickSound { get; private set; }
    [field: SerializeField] public float ClickVolume { get; private set; } = 1f;

    [field: SerializeField] public AudioClip BuyUpgradeSound { get; private set; }
    [field: SerializeField] public float BuyUpgradeVolume { get; private set; } = 1f;

    [field: SerializeField] public AudioClip ThoughtDestroySound { get; private set; }
    [field: SerializeField] public float ThoughtDestroyVolume { get; private set; } = 1f;
}
