using Cysharp.Threading.Tasks;
using DG.Tweening;
using Main.Scripts.Views;
using Zenject;

public class MindLevelService : IInitializable, IMindLevelService
{
    private readonly MindView view;
    private readonly Mind mind;
    private readonly MindData data;
    private readonly AudioPlayer audio;
    private readonly UpgradeMaterialAnimation animation;
    private readonly IMindProgressService progress;
    private readonly ThoughtSpawner spawner;

    public MindLevelService(
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

    public void Initialize()
    {
        progress.Redraw();
        RedrawMindLevel((mind.Level + 1).ToString());
    }

    //TODO
    public async UniTask LevelUp()
    {
        progress.BlockFarming(true);
        progress.StopFarming();

        await view.ProgressBar.DOFillAmount(1, .3f).AsyncWaitForCompletion().AsUniTask();
        await view.ProgressBar.transform.DOShakeRotation(.3f, 10, 5, 15, true).AsyncWaitForCompletion().AsUniTask();

        audio.PlaySFX(data.UpgradeSound, data.SoundVolume);
        await animation.UpgradeColorAnimation();

        view.ProgressBar.fillAmount = 0;
        progress.Redraw();
        RedrawMindLevel((mind.Level + 1).ToString());

        spawner.Spawn();
        progress.BlockFarming(false);
    }

    public async UniTask LevelReduce()
    {
        progress.BlockFarming(false);
        progress.StopFarming();
        RedrawMindLevel((mind.Level + 1).ToString());
        await animation.Reduce();
    }

    private void RedrawMindLevel(string text)
    {
        view.MindLevelText.text = text;
    }
}
