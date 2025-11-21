using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

public class MindController : IInitializable, ITickable, IDisposable
{
    private readonly Mind mind;
    private readonly ThoughtSpawner spawner;
    private readonly IMindProgressService progress;
    private readonly IMindLevelService level;

    public MindController(
        Mind mind,
        ThoughtSpawner spawner,
        IMindProgressService progress,
        IMindLevelService levelUp)
    {
        this.mind = mind;
        this.spawner = spawner;
        this.progress = progress;
        this.level = levelUp;
    }

    public void Initialize()
    {
        spawner.OnDestroy += _ => progress.StartFarming();
        mind.OnLevelUp += () => level.LevelUp().Forget();
        mind.OnLevelReduce += () => level.LevelReduce().Forget();
    }

    public void Dispose()
    {
        spawner.OnDestroy -= _ => progress.StartFarming();
        mind.OnLevelUp -= () => level.LevelUp().Forget();
        mind.OnLevelReduce -= () => level.LevelReduce().Forget();
    }

    public void Tick()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            level.LevelUp().Forget();
        }
#endif
    }
}
