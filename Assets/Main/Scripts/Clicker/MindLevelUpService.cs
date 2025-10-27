using Cysharp.Threading.Tasks;
using DG.Tweening;
using Main.Scripts.Views;

public class MindLevelUpService : IMindLevelUpService
{
    private readonly MindView view;
    private readonly Mind mind;
    private readonly MindData data;
    private readonly AudioPlayer audio;
    private readonly UpgradeMaterialAnimation animation;
    private readonly IMindProgressService progress;
    private readonly ThoughtSpawner spawner;

    public MindLevelUpService(
        MindView view,
        Mind mind,
        MindData data,
        AudioPlayer audio,
        UpgradeMaterialAnimation animation,
        IMindProgressService progress,
        ThoughtSpawner spawner)
    {
        this.view = view;
        this.mind = mind;
        this.data = data;
        this.audio = audio;
        this.animation = animation;
        this.progress = progress;
        this.spawner = spawner;
    }

    public async UniTask PlayLevelUp()
    {
        progress.BlockFarming(true);
        progress.StopFarming();

        await view.ProgressBar.DOFillAmount(1, .3f).AsyncWaitForCompletion().AsUniTask();
        await view.ProgressBar.transform.DOShakeRotation(.3f, 10, 5, 15, true).AsyncWaitForCompletion().AsUniTask();

        audio.PlaySFX(data.UpgradeSound, data.SoundVolume);
        await animation.Apply();

        view.ProgressBar.fillAmount = 0;
        progress.Redraw();

        view.MindLevelText.text = (mind.Level + 1).ToString();

        spawner.Spawn();
        progress.BlockFarming(false);
    }
}
