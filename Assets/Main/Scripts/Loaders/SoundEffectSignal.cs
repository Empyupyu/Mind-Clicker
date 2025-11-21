using UnityEngine;

public class SoundEffectSignal
{
    public AudioClip AudioClip { get; }
    public float Volume { get; }
    public SoundEffectSignal(AudioClip audioClip, float volume)
    {
        AudioClip = audioClip;
        Volume = volume;
    }
}
