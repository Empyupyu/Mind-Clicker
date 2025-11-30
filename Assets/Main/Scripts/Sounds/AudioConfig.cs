using UnityEngine;

[CreateAssetMenu(fileName = "AudioConfig", menuName = "Create Game Datas/AudioConfig")]
public class AudioConfig : ScriptableObject
{
    [field: SerializeField] public AudioClip[] MusicClips { get; private set; }
    [field: SerializeField] public AudioClip OpeningAmbiantClip { get; private set; }
    [field: SerializeField] public float OpeningAmbiantClipStartTime { get; private set; } = 0.04f;
    [field: SerializeField] public float LevelAmbiantStartEarly { get; private set; } = 0.01f;
    [field: SerializeField] public float LevelAmbiantStartFull { get; private set; } = 1f;
    [field: SerializeField] public float LevelAmbiantStartMid { get; private set; } = 0.7f;
    [field: SerializeField] public int OpeningDelay { get; private set; } = 7500;
    [field: SerializeField] public int LoopMusicDelay { get; private set; } = 15000;
    [field: SerializeField] public AudioClip LevelAmbiantClip { get; private set; }
    [field: SerializeField] public float FadeOutDuration { get; private set; } = 1f;
}
