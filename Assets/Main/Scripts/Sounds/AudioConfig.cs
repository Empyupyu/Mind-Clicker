using UnityEngine;

[CreateAssetMenu(fileName = "AudioConfig", menuName = "Create Game Datas/AudioConfig")]
public class AudioConfig : ScriptableObject
{
    [field: SerializeField] public AudioClip[] MusicClips { get; private set; }
    [field: SerializeField] public AudioClip OpeningAmbiantClip { get; private set; }
    [field: SerializeField] public AudioClip LevelAmbiantClip { get; private set; }
    [field: SerializeField] public float FadeOutDuration { get; private set; } = 1f;
}
