using System;
using Zenject;

public class UpgradeSoundFeedbackService : IInitializable, IDisposable
{
    private readonly Upgrade upgrade;
    private readonly AudioPlayer audioPlayer;
    private readonly SoundConfig soundConfig;

    public UpgradeSoundFeedbackService(Upgrade upgrade, AudioPlayer audioPlayer, SoundConfig soundConfig)
    {
        this.upgrade = upgrade;
        this.audioPlayer = audioPlayer;
        this.soundConfig = soundConfig;
    }

    public void Initialize()
    {
        upgrade.OnUpgrade += PlayUpgradeEffect;
    }

    private void PlayUpgradeEffect(UpgradeData upgradeData)
    {
        audioPlayer.PlaySFX(soundConfig.BuyUpgradeSound, soundConfig.BuyUpgradeVolume);
    }

    public void Dispose()
    {
        upgrade.OnUpgrade -= PlayUpgradeEffect;
    }
}
