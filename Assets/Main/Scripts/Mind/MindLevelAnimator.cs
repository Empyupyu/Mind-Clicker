using Cysharp.Threading.Tasks;
using DG.Tweening;
using Main.Scripts.Views;

public class MindLevelAnimator : IMindLevelAnimator
{
    public async UniTask PlayLevelUpAnimation(MindView view, AudioPlayer audio, MindData data, MindLevelUpAnimation animation)
    {
        await view.ProgressBar.DOFillAmount(data.LevelUpProgressBarTargetValue, data.LevelUpProgressBarDuration).AsyncWaitForCompletion().AsUniTask();
        await view.ProgressBar.transform.DOShakeRotation(
            data.LevelUpProgressBarShakeDuration,
            data.LevelUpProgressBarShakeStrength, 
            data.LevelUpProgressBarShakeVibrato, 
            data.LevelUpProgressBarShakeRandomness, 
            true).AsyncWaitForCompletion().AsUniTask();

        audio.PlaySFX(data.UpgradeSound, data.SoundVolume);
        await animation.UpgradeColorAnimation();

        view.ProgressBar.fillAmount = 0;
    }

    public async UniTask PlayLevelReduceAnimation(MindView view, MindLevelUpAnimation animation)
    {
        await animation.Reduce();
    }
}