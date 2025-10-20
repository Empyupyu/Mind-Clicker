using Zenject;

public class DamageFeedbackService : IInitializable
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
        dealDamage.OnClickDamage += () =>
        {
            audioPlayer.PlaySFX(soundConfig.ClickSound, soundConfig.ClickVolume);
        };

        thoughtSpawner.OnDestroy += () =>
        {
            audioPlayer.PlaySFX(soundConfig.ThoughtDestroySound, soundConfig.ThoughtDestroyVolume);
        };
    }
}