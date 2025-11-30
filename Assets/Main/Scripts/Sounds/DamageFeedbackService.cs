using System;
using Zenject;

public class DamageFeedbackService : IInitializable, IDisposable
{
    private readonly ThoughtDamageService thoughtDamageService;
    private readonly ThoughtSpawner thoughtSpawner;
    private readonly AudioPlayer audioPlayer;
    private readonly SoundConfig soundConfig;

    public DamageFeedbackService(
        ThoughtDamageService thoughtDamageService,
        ThoughtSpawner thoughtSpawner,
        AudioPlayer audioPlayer,
        SoundConfig soundConfig)
    {
        this.thoughtDamageService = thoughtDamageService;
        this.thoughtSpawner = thoughtSpawner;
        this.audioPlayer = audioPlayer;
        this.soundConfig = soundConfig;
    }

    public void Initialize()
    {
        thoughtDamageService.OnClickDamage += PlayClickEffect;
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
        thoughtDamageService.OnClickDamage -= PlayClickEffect;
        thoughtSpawner.OnDestroy -= PlayDestroyEffect;
    }
}