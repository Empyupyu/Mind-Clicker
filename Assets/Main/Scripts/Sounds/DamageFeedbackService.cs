using System;
using Zenject;

public class DamageFeedbackService : IInitializable, IDisposable
{
    private readonly DealDamage dealDamage;
    private readonly ThoughtSpawner thoughtSpawner;
    private readonly AudioPlayer audioPlayer;
    private readonly SoundConfig soundConfig;

    public DamageFeedbackService(DealDamage dealDamage, ThoughtSpawner thoughtSpawner, AudioPlayer audioPlayer, SoundConfig soundConfig)
    {
        this.dealDamage = dealDamage;
        this.thoughtSpawner = thoughtSpawner;
        this.audioPlayer = audioPlayer;
        this.soundConfig = soundConfig;
    }

    public void Initialize()
    {
        dealDamage.OnClickDamage += PlayClickEffect;
        thoughtSpawner.OnDestroy += PlayDestroyEffect;
    }

    private void PlayDestroyEffect(NegativeThought negativeThought)
    {
        audioPlayer.PlaySFX(soundConfig.ThoughtDestroySound, soundConfig.ThoughtDestroyVolume);
    }

    private void PlayClickEffect()
    {
        audioPlayer.PlaySFX(soundConfig.ClickSound, soundConfig.ClickVolume);
    }

    public void Dispose()
    {
        dealDamage.OnClickDamage -= PlayClickEffect;
        thoughtSpawner.OnDestroy -= PlayDestroyEffect;
    }
}