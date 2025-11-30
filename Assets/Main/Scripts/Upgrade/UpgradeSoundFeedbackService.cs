using System;
using Zenject;

public class UpgradeSoundFeedbackService : IInitializable, IDisposable
{
    private readonly UpgradeService upgradeService;
    private readonly AudioPlayer audioPlayer;
    private readonly SoundConfig soundConfig;

    public UpgradeSoundFeedbackService(UpgradeService upgradeService, AudioPlayer audioPlayer, SoundConfig soundConfig)
    {
        this.upgradeService = upgradeService;
        this.audioPlayer = audioPlayer;
        this.soundConfig = soundConfig;
    }

    public void Initialize()
    {
        upgradeService.OnUpgrade += PlayUpgradeEffect;
    }

    private void PlayUpgradeEffect(Upgrade upgrade)
    {
        audioPlayer.PlaySFX(soundConfig.BuyUpgradeSound, soundConfig.BuyUpgradeVolume);
    }

    public void Dispose()
    {
        upgradeService.OnUpgrade -= PlayUpgradeEffect;
    }
}
