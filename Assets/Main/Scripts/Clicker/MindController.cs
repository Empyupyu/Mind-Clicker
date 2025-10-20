using Cysharp.Threading.Tasks;
using DG.Tweening;
using Main.Scripts.Views;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

public class MindController : IInitializable, IDisposable, ITickable
{
    private readonly PlayerData playerData;
    private readonly MindView mindView;
    private readonly ThoughtSpawner thoughtSpawner;
    private readonly Mind mind;
    private readonly MindData mindData;
    private readonly AudioPlayer audioPlayer;
    private readonly UpgradeMaterialAnimation upgradeMaterialAnimation;

    private CancellationTokenSource mindPointCts;

    private bool isLevelUp;

    public MindController(PlayerData playerData, MindView mindView, ThoughtSpawner thoughtSpawner, Mind mind,  MindData mindData, AudioPlayer audioPlayer, UpgradeMaterialAnimation upgradeMaterialAnimation)
    {
        this.playerData = playerData;
        this.mindView = mindView;
        this.thoughtSpawner = thoughtSpawner;
        this.mind = mind;
        this.mindData = mindData;
        this.audioPlayer = audioPlayer;
        this.upgradeMaterialAnimation = upgradeMaterialAnimation;
    }

    public void Initialize()
    {
        RedrawProgress(playerData.MindPoints / mind.PointForLevelUp);
        RedrawMindLevel();

        thoughtSpawner.OnDestroy += StartFarmingMindPoints;
        mind.OnLevelUp += LevelUpTransition;
        mind.OnLevelReduce += RedrawMindLevel;
    }

    private void StartFarmingMindPoints(NegativeThought negativeThought = null)
    {
        mindPointCts = new CancellationTokenSource();
        FarmingMindPoints(mindPointCts.Token).Forget();
    }

    private async UniTask FarmingMindPoints(CancellationToken token)
    {
        while (!HasBadThoughts() && !isLevelUp)
        {
            if (token.IsCancellationRequested) return;

            RedrawProgress(mind.ProgressPercent);

            mind.AddMindPoints(Time.deltaTime);

            await UniTask.Yield();
        }
    }

    private bool HasBadThoughts()
    {
        return thoughtSpawner.GetTargetThought() != null;
    }

    private void LevelUpTransition()
    {
        LevelUp().Forget();
    }

    private async UniTask LevelUp()
    {
        isLevelUp = true;
        mindPointCts?.Cancel();

        await mindView.ProgressBar.DOFillAmount(1, .3f).AsyncWaitForCompletion().AsUniTask();

        await mindView.ProgressBar.transform.DOShakeRotation(.3f, 10, 5, 15, true).AsyncWaitForCompletion().AsUniTask();

        audioPlayer.PlaySFX(mindData.UpgradeSound, mindData.SoundVolume);

        await upgradeMaterialAnimation.Apply();

        mindView.ProgressBar.fillAmount = 0;

        RedrawMindPoints();
        RedrawMindLevel();
        thoughtSpawner.SpawnThought();

        isLevelUp = false;
    }

    private void RedrawProgress(float progress)
    {
        mindView.ProgressBar.fillAmount = progress;
        RedrawMindPoints();
    }

    private void RedrawMindPoints()
    {
        mindView.ProgressText.text = string.Format("{00:0.0}", playerData.MindPoints) + "/" + string.Format("{00:0.0}", mind.PointForLevelUp);
    }

    private void RedrawMindLevel()
    {
        mindView.MindLevelText.text = "" + (playerData.MindLevel + 1);
    }

    public void Dispose()
    {
        thoughtSpawner.OnDestroy -= StartFarmingMindPoints;
        mind.OnLevelUp -= LevelUpTransition;
        mind.OnLevelReduce -= RedrawMindLevel;
    }

    public void Tick()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LevelUp();
        }
#endif
    }
}
