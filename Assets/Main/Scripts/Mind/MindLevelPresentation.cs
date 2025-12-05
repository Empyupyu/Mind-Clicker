using Cysharp.Threading.Tasks;
using Main.Scripts.Views;
using System;
using Zenject;

public class MindLevelPresentation : IInitializable, IMindLevelPresentation
{
    public event Action OnLevelUpAnimationEnded;
    public event Action OnLevelReduceAnimationEnded;

    private readonly MindView view;
    private readonly MindProgress mindProgress;
    private readonly MindData data;
    private readonly AudioPlayer audio;
    private readonly MindLevelUpAnimation animation;
    private readonly IMindProgressUpdater progress;
    private readonly IMindLevelAnimator mindLevelAnimator;

    private string DisplayLevel () => (mindProgress.Level + 1).ToString();

    public MindLevelPresentation(
        MindView view,
        MindProgress mindProgress,
        MindData data,
        AudioPlayer audio,
        MindLevelUpAnimation animation,
        IMindProgressUpdater progress,
        IMindLevelAnimator mindLevelAnimator)
    {
        this.view = view;
        this.mindProgress = mindProgress;
        this.data = data;
        this.audio = audio;
        this.animation = animation;
        this.progress = progress;
        this.mindLevelAnimator = mindLevelAnimator;
    }

    public void Initialize()
    {
        progress.Redraw();
        RedrawMindLevel();
    }

    public async UniTask LevelUp()
    {
        BlockProgress();

        progress.StopFarming();

        await mindLevelAnimator.PlayLevelUpAnimation(view, audio, data, animation);

        progress.Redraw();
        RedrawMindLevel();

        OnLevelUpAnimationEnded?.Invoke();

        UnblockProgress();
    }

    private void BlockProgress() => progress.BlockFarming(true);
    private void UnblockProgress() => progress.BlockFarming(false);

    public async UniTask LevelReduce()
    {
        BlockProgress();

        progress.StopFarming();
        RedrawMindLevel();

        await mindLevelAnimator.PlayLevelReduceAnimation(view, animation);

        OnLevelReduceAnimationEnded?.Invoke();
        UnblockProgress();
    }

    private void RedrawMindLevel()
    {
        view.MindLevelText.text = DisplayLevel();
    }
}
