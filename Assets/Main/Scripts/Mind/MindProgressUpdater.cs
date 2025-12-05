using Cysharp.Threading.Tasks;
using Main.Scripts.Views;
using System;
using System.Threading;
using UnityEngine;

public class MindProgressUpdater : IMindProgressUpdater, IDisposable
{
    private readonly PlayerDataRef playerData;
    private readonly MindProgress mindProgress;
    private readonly MindView view;
    private readonly ThoughtSpawner spawner;

    private CancellationTokenSource farimngCts;
    private bool isBlockFarming;

    public MindProgressUpdater(
        PlayerDataRef playerData,
        MindProgress mindProgress,
        MindView view,
        ThoughtSpawner spawner)
    {
        this.playerData = playerData;
        this.mindProgress = mindProgress;
        this.view = view;
        this.spawner = spawner;
    }

    public void StartFarming()
    {
        if (isBlockFarming || HasBadThoughts()) return;

        farimngCts = new CancellationTokenSource();
        FarmingLoop(farimngCts.Token).Forget();
    }

    public void StopFarming()
    {
        farimngCts?.Cancel();
    }

    public void Redraw()
    {
        float percent = playerData.Value.MindPoints / mindProgress.PointForLevelUp;
        view.ProgressBar.fillAmount = percent;

        view.ProgressText.text = $"{playerData.Value.MindPoints.ToAbbreviatedString()}/" +
                                 $"{mindProgress.PointForLevelUp.ToAbbreviatedString()}";
    }

    public void BlockFarming(bool isBlock)
    {
        isBlockFarming = isBlock;
    }

    private async UniTask FarmingLoop(CancellationToken token)
    {
        while (!HasBadThoughts() && !token.IsCancellationRequested)
        {
            Redraw();
            mindProgress.AddMindPoints(Time.deltaTime);

            await UniTask.Yield();
        }
    }

    private bool HasBadThoughts() => spawner.GetTarget() != null;

    public void Dispose()
    {
        farimngCts?.Cancel();
    }
}
