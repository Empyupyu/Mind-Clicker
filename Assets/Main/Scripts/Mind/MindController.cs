using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

public class MindController : IInitializable, ITickable, IDisposable
{
    private readonly MindProgress mindProgress;
    private readonly ThoughtSpawner spawner;
    private readonly IMindProgressUpdater mindProgressUpdater;
    private readonly IMindLevelPresentation mindLevelPresentation;

    public MindController(
        MindProgress mindProgress,
        ThoughtSpawner spawner,
        IMindProgressUpdater mindProgressUpdater,
        IMindLevelPresentation mindLevelPresentation)
    {
        this.mindProgress = mindProgress;
        this.spawner = spawner;
        this.mindProgressUpdater = mindProgressUpdater;
        this.mindLevelPresentation = mindLevelPresentation;
    }

    public void Initialize()
    {
        spawner.OnDestroy += _ => mindProgressUpdater.StartFarming();
        mindProgress.OnLevelUp += () => mindLevelPresentation.LevelUp().Forget();
        mindProgress.OnLevelReduce += () => mindLevelPresentation.LevelReduce().Forget();
    }

    public void Dispose()
    {
        spawner.OnDestroy -= _ => mindProgressUpdater.StartFarming();
        mindProgress.OnLevelUp -= () => mindLevelPresentation.LevelUp().Forget();
        mindProgress.OnLevelReduce -= () => mindLevelPresentation.LevelReduce().Forget();
    }

    public void Tick()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            mindLevelPresentation.LevelUp().Forget();
        }
#endif
    }
}
