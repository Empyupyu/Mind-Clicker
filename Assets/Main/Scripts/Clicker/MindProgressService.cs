using Cysharp.Threading.Tasks;
using Main.Scripts.Views;
using System;
using System.Threading;
using UnityEngine;

public class MindProgressService : IMindProgressService, IDisposable
{
    private readonly PlayerDataRef playerData;
    private readonly Mind mind;
    private readonly MindView view;
    private readonly ThoughtSpawner spawner;

    private CancellationTokenSource farimngCts;
    private bool isFarming;
    private bool isBlockFarming;

    public MindProgressService(PlayerDataRef playerData, Mind mind, MindView view, ThoughtSpawner spawner)
    {
        this.playerData = playerData;
        this.mind = mind;
        this.view = view;
        this.spawner = spawner;
    }

    public void StartFarming()
    {
        if (isFarming || isBlockFarming) return;
        isFarming = true;

        farimngCts = new CancellationTokenSource();
        FarmingLoop(farimngCts.Token).Forget();
    }

    public void StopFarming()
    {
        isFarming = false;
        farimngCts?.Cancel();
    }

    public void Redraw()
    {
        float progress = playerData.Value.MindPoints / mind.PointForLevelUp;
        view.ProgressBar.fillAmount = progress;

        view.ProgressText.text = $"{playerData.Value.MindPoints.ToAbbreviatedString()}/" +
                                 $"{mind.PointForLevelUp.ToAbbreviatedString()}";
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
            mind.AddMindPoints(Time.deltaTime);

            await UniTask.Yield();
        }
    }

    private bool HasBadThoughts() => spawner.GetTarget() != null;

    public void Dispose()
    {
        farimngCts?.Cancel();
    }
}
